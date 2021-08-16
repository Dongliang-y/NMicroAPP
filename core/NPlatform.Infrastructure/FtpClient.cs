#region 说明
/*
 * 实现向FTP下载文件和上传文件等功能
 * Version：V1.0  作者：汤红燕   日期:2007-05-08
 * 
 * 
 * 2007-08-29 
 * 可能会有影响的几处
 * Encoding的编码方式ASCII还是Default
 * ReadLine的线程延迟
 * 获取返回码的result和ResultCode的处理
 * 读取到返回码后的处理情况
 */
#endregion

using System;	
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Messaging;
using System.Threading;

#region ftp应答码及其意义
/*
//--------------------------------------------------------------------------------
110 Restart marker reply.
重新启动标记应答。在这种情况下文本是确定的，它必须是：MARK yyyy=mmmm，其中yyyy 
是用户进程数据流标记，mmmm是服务器标记。 

120 Service ready in nnn minutes.
服务在nnn分钟内准备好 

125 Data connection already open; transfer starting.
数据连接已打开，准备传送 

150 File status okay; about to open data connection.
文件状态良好，打开数据连接 


200 Command okay.
命令成功 

202 Command not implemented, superfluous(过剩的,多余的) at this site.
命令未实现 

211 System status, or system help reply.
系统状态或系统帮助响应 

212 Directory status.
目录状态 

213 File status.
文件状态 

214 Help message.
帮助信息，信息仅对人类用户有用 

215 NAME system type.
名字系统类型 

220 Service ready for new user.
对新用户服务准备好 

221 Service closing control connection.
服务关闭控制连接，可以退出登录 

225 Data connection open; no transfer in progress.
数据连接打开，无传输正在进行 

226 Closing data connection.


227 Entering Passive Mode (h1,h2,h3,h4,p1,p2).
进入被动模式 

230 User logged in, proceed.
用户登录 

250 Requested file action okay, completed.
请求的文件操作完成
 

请求的文件操作完成 
257 "PATHNAME" created.
创建"PATHNAME" 

331 User name okay, need password.
用户名正确，需要口令 

332 Need account for login.
登录时需要帐户信息 

350 Requested file action pending further information.
请求的文件操作需要进一步命令 

421 Service not available, closing control connection.
不能提供服务，关闭控制连接 

425 Can't open data connection.
不能打开数据连接 

426 Connection closed; transfer aborted.
关闭连接，中止传输 

450 Requested file action not taken.
请求的文件操作未执行 

451 Requested action aborted: local error in processing.
中止请求的操作：有本地错误 

452 Requested action not taken.
未执行请求的操作：系统存储空间不足 

500 Syntax error, command unrecognized.
格式错误，命令不可识别 

501 Syntax error in parameters or arguments.
参数语法错误 

502 Command not implemented.
命令未实现 

503 Bad sequence of commands.
命令顺序错误 

504 Command not implemented for that parameter.
此参数下的命令功能未实现 

530 Not logged in.
未登录 

532 Need account for storing files
存储文件需要帐户信息 

550 Requested action not taken.
未执行请求的操作 

551 Requested action aborted: page type unknown.
请求操作中止：页类型未知 

552 Requested file action aborted.
请求的文件操作中止，存储分配溢出 

553 Requested action not taken.
未执行请求的操作：文件名不合法
*/
#endregion

namespace ZJJWEPlatform.Infrastructure
{
    /// <summary>
    /// ftp客户端
    /// </summary>
	public class FtpClient
	{

		public class FtpException : Exception
		{
			public FtpException(string message) : base(message)
			{
			}

			public FtpException(string message, Exception innerException) : base(message,innerException)
			{
			}
		}

		private static int BUFFER_SIZE = 512;
		private static Encoding ASCII = Encoding.Default;

		private bool verboseDebugging = true; //false;

		// defaults
		private string server = "localhost";
		private string remotePath = ".";
		private string username = "anonymous";
		private string password = "anonymous@anonymous.net";
		private string message = null;
		private string result = null;
		private int m_iServerOS=32;

		private int port = 21;
		private int bytes = 0;
		private int resultCode = 0;

		private bool loggedin = false;
		private bool binMode = false;

		private Byte[] buffer = new Byte[BUFFER_SIZE];
		private Byte[] receiveBuffer = new Byte[BUFFER_SIZE*2];
		private Socket clientSocket = null;

		private int timeoutSeconds = 10;



		#region 连接属性
		//服务器名称
		public string Server
		{
			get
			{
				return this.server;
			}
			set
			{
				this.server = value;
			}
		}
		
		//用户名
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
			}
		}
		
		//密码
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		//端口号，通常是21
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
		
		//连接最大延迟 
		public int Timeout
		{
			get
			{
				return this.timeoutSeconds;
			}
			set
			{
				this.timeoutSeconds = value;
			}
		}
		#endregion

		#region 其他属性
		/// <summary>
		/// Display all communications to the debug log
		/// </summary>
		public bool VerboseDebugging
		{
			get
			{
				return this.verboseDebugging;
			}
			set
			{
				this.verboseDebugging = value;
			}
		}

		/// <summary>
		/// Display Server OS
		/// </summary>
		public int ServerOS 
		{
			get { return m_iServerOS;}
		}

		//登录的目录
		public string RemotePath
		{
			get
			{
				return this.remotePath;
			}
			set
			{
				this.remotePath = value;
			}
		}
		
		//下载文件时是否采用二进制方式
		public bool BinaryMode
		{
			get
			{
				return this.binMode;
			}
			set
			{
				if ( this.binMode == value ) return;

				if ( value )
					sendCommand("TYPE I");

				else
					sendCommand("TYPE A");

				if ( this.resultCode != 200 ) 
				{
					FireException(result);
				}
			}
		}
		#endregion

		#region 构造函数
		//		/// <summary>
		//		/// Default contructor
		//		/// </summary>
		//		public FtpClient()
		//		{
		//		}
		//		/// <summary>
		//		/// 
		//		/// </summary>
		//		/// <param name="server"></param>
		//		/// <param name="username"></param>
		//		/// <param name="password"></param>
		//		public FtpClient(string server, string username, string password)
		//		{
		//			this.server = server;
		//			this.username = username;
		//			this.password = password;
		//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="timeoutSeconds"></param>
		/// <param name="port"></param>
		public FtpClient(string server, string username, string password, int timeoutSeconds, int port)
		{
			this.server = server;
			this.username = username;
			this.password = password;
			this.timeoutSeconds = timeoutSeconds;
			this.port = port;

			string LogFilePath = @"FtpData\FtpLog.txt";
			if(File.Exists(LogFilePath))
			{
				FileInfo LogFinInfo = new FileInfo(LogFilePath);
				long Len = LogFinInfo.Length;
				//如果文件大小超过1M,清空文件内容
				if(Len>16)
				{
					LogFinInfo.Delete();
				}
			}
		}
		#endregion


//-----------------------------------------------------------------------------//
		//登录部分
		#region 登录FTP服务器
		public bool Login()
		{
			if (this.loggedin)
			{
				this.Close();
			}

			IPAddress addr = null;
			IPEndPoint ep = null;
			try
			{
				this.clientSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
				addr = Dns.Resolve(this.server).AddressList[0];
				ep = new IPEndPoint( addr, this.port );
				//开始连接
				this.clientSocket.Connect(ep);
			}
			catch(Exception ex)
			{
				if ( this.clientSocket != null && this.clientSocket.Connected ) this.clientSocket.Close();
				//连接的时候发生了异常
				FireException("Couldn't connect to remote server",ex);
				return false; 
			}
			//读取连接后的返回码
			this.readResponse();

			//连接时，返回码220表示命令成功
			if(this.resultCode != 220)
			{
				this.Close();
				if(this.result==null)
				{
					FireException("不能和Ftp服务器相连接!");
				}
				else
				{
					FireException(this.result);
				}
				return false; 
			}

			/// <summary> 
			/// Sends a user name
			/// </summary>
			/// <remarks>Reply codes: 230 530 500 501 421 331 332</remarks>
			this.sendCommand( "USER " + username );

			if( !(this.resultCode == 331 || this.resultCode == 230) )
			{
				this.cleanup();
				FireException(this.result);
				return false; 
			}

			if( this.resultCode != 230 )
			{
				/// <summary>
				/// send the user's password
				/// </summary>
				/// <remarks>Reply codes: 230 202 530 500 501 503 421 332</remarks>
				this.sendCommand( "PASS " + password );

				if( !(this.resultCode == 230 || this.resultCode == 202) )
				{
					this.cleanup();
					FireException(this.result);
					return false; 
				}
			}

			this.loggedin = true;
            return true;
		}
		#endregion
	
		#region 关闭和FTP的连接
		public void Close()
		{
			if( this.clientSocket != null )
			{
				/// <summary>
				/// This command terminates a USER and if file transfer is not in progress, the server closes the control connection. 
				/// </summary>
				/// <remarks>Reply codes: 221 500</remarks>
				this.sendCommand("QUIT");
			}

			this.cleanup();
		}
		#endregion
//-----------------------------------------------------------------------------//



//-----------------------------------------------------------------------------//
		//PART1：文件操作部分
		//1.下载文件
		#region 下载FTP目录下的某个文件
		public void Download(string remFileName)
		{
			this.Download(remFileName,"",false);
		}
		#endregion
		
		#region 下载文件到本地，确定是否断点下载
		public void Download(string remFileName,Boolean resume)
		{
			this.Download(remFileName,"",resume);
		}
		#endregion
		
		#region 下载文件到本地，本地文件目录必须存在，文件如果不存在则创建
		public void Download(string remFileName,string locFileName)
		{
			this.Download(remFileName,locFileName,false);
		}
		#endregion

		#region 下载文件到本地，本地文件目录必须存在，文件如果不存在则创建.可以使用断点下载
		public void Download(string remFileName,string locFileName,Boolean resume)
		{
			if (!this.loggedin) 
			{
				this.Login();
			}
			if (locFileName.Equals(""))
			{
				locFileName = remFileName;
			}

			FileStream output = null;

            if (!File.Exists(locFileName))
                output = File.Create(locFileName);

            else
                output = new FileStream(locFileName, FileMode.Create);
        

			Socket cSocket = createDataSocket();
			//创建数据连接后，为什么不要对返回码进行判断
			//readResponse();

			long offset = 0;

			if ( resume )
			{
				offset = output.Length;

				if ( offset > 0 )
				{

					/// <summary>
					/// The argument field represents the server marker at which file transfer is to be
					/// restarted. This command does not cause file transfer but skips over the file to
					/// the specified data checkpoint.
					/// </summary>
					/// <remarks>Reply codes: 500 501 502 421 530 350</remarks>
					this.sendCommand( "REST " + offset );
					if ( this.resultCode != 350 )
					{
						//Server dosnt support resuming
						offset = 0;
                    }
					else
					{
						output.Seek( offset, SeekOrigin.Begin );
					}
				}
			}

			/// <summary>
			/// Causes the server-DTP to transfer a copy of the file, specified 
			/// in the pathname, to the server- or user-DTP at the other end of the data 
			/// connection. The status and contents of the file at the server site shall be unaffected.
			/// </summary>
			/// <remarks>Reply codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530</remarks>
			this.sendCommand("RETR " + remFileName);

			/*	125,150,110,250,226 (success) */
			if ( this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226 )
			{
				output.Close();

				if ( cSocket.Connected )
				{
					cSocket.Close();
				}
				FireException(this.result);
			}

			DateTime timeout = DateTime.Now.AddSeconds(this.timeoutSeconds);

			while ( timeout > DateTime.Now )
			{
				this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
				output.Write(this.buffer,0,this.bytes);

				if ( this.bytes <= 0)
				{
					break;
				}
			}

			//close the iostream and datasocket
			output.Close();
			if ( cSocket.Connected )
			{
				cSocket.Close();
			}

			this.readResponse();

			if( this.resultCode != 226 && this.resultCode != 250 && this.result!=null)
			{
				//看了网上的一段代码后，如果读取的值不是226或者250，再读取一次？？
				//FireException(this.result.Substring(4));
//				FireException(this.result);
//				showMessage(this.result,true);
				if( this.resultCode != 226 && this.resultCode != 250 && this.result!=null)
				{
					FireException(this.result);
				}

			}
		}

		#endregion


		//2.上传文件
		#region 上传文件
		public void Upload(string fileName)
		{
			this.Upload(fileName,false);
		}
		#endregion
		
		#region 上传文件，可以使用断点上传
		public void Upload(string fileName, bool resume)
		{
			if ( !this.loggedin ) 
			{
				this.Login();
			}

			Socket cSocket = null ;
			long offset = 0;

			if ( resume )
			{
				try
				{
					this.BinaryMode = true;

					offset = GetFileSize( Path.GetFileName(fileName) );
				}
				catch(Exception)
				{
					// file not exist
					offset = 0;
				}
			}

			// open stream to read file
			FileStream input = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read);

			if ( resume && input.Length < offset )
			{
				// different file size
				offset = 0;
			}
			else if ( resume && input.Length == offset )
			{
				// file done
				input.Close();
                return;
			}

			// dont create untill we know that we need it
			cSocket = this.createDataSocket();

			if ( offset > 0 )
			{
				/// <summary>
				/// The argument field represents the server marker at which file transfer is to be
				/// restarted. This command does not cause file transfer but skips over the file to
				/// the specified data checkpoint.
				/// </summary>
				/// <remarks>Reply codes: 500 501 502 421 530 350</remarks>
				this.sendCommand( "REST " + offset );
				if ( this.resultCode != 350 )
				{
					offset = 0;
				}
			}

			/// <summary>
			/// Causes the server-DTP to accept the data transferred via the data connection 
			/// and to store the data as a file at the server site. If the file specified in 
			/// the pathname exists at the server site, then its contents shall be replaced 
			/// by the data being transferred. A new file is created at the server site if 
			/// the file specified in the pathname does not already exist.
			/// </summary>
			/// <remarks>Reply codes: 125 150 110 226 250 425 426 451 551 552 532 450 452 553 500 501 421 530</remarks>
			this.sendCommand( "STOR " + Path.GetFileName(fileName) );

			if ( this.resultCode != 125 && this.resultCode != 150 ) 
			{
				input.Close();
				if(result!=null)
				{
					FireException("After Store Command:"+result.ToString());
				}
				else
				{
					FireException("After Store Command:"+resultCode.ToString());
				}
			}

			if ( offset != 0 )
			{
				input.Seek(offset,SeekOrigin.Begin);
			}
			while ((bytes = input.Read(buffer,0,buffer.Length)) > 0)
			{
				cSocket.Send(buffer, bytes, 0);
			}
			
			input.Close();

			if (cSocket.Connected)
			{
				cSocket.Close();
			}

			this.readResponse();

			
			//2007-08-17 测试办公室的服务器后发现返回码是150 
			if( this.resultCode != 226 && this.resultCode != 250 && this.resultCode != 150) 
			{
				if(result!=null)
				{
					FireException("After Data Uploaded:"+result.ToString());
				}
				else
				{
					FireException("After Data Uploaded:"+resultCode.ToString());
				}
			}
		}
		#endregion
		
		#region 上传某个目录下的文件
		/// <summary>
		/// Upload a directory and its file contents
		/// </summary>
		/// <param name="path"></param>
		/// <param name="recurse">Whether to recurse sub directories</param>
		public void UploadDirectory(string path, bool recurse)
		{
			this.UploadDirectory(path,recurse,"*.*");
		}
		#endregion
		
		#region 上传某个目录下的文件
		/// <summary>
		/// Upload a directory and its file contents
		/// </summary>
		/// <param name="path"></param>
		/// <param name="recurse">Whether to recurse sub directories</param>
		/// <param name="mask">Only upload files of the given mask - everything is '*.*'</param>
		public void UploadDirectory(string path, bool recurse, string mask)
		{
			string[] dirs = path.Replace("/",@"\").Split('\\');
			string rootDir = dirs[ dirs.Length - 1 ];

			// make the root dir if it doed not exist
			try
			{
				// try to retrieve files
				this.GetFileList(rootDir);
			}
			catch
			{
				// if receive an error
				this.MakeDir(rootDir);
			}

//			if ( this.GetFileList(rootDir).Length < 1 ) this.MakeDir(rootDir);

			this.ChangeDir(rootDir);

			foreach ( string file in Directory.GetFiles(path,mask) )
			{
				this.Upload(file,true);
			}
			if ( recurse )
			{
				foreach ( string directory in Directory.GetDirectories(path) )
				{
					this.UploadDirectory(directory,recurse,mask);
				}
			}

			this.ChangeDir("..");
		}
		#endregion

#region 以下部分未使用
		//3.删除文件
		#region 删除某个文件
		/// <summary>
		/// Delete a file from the remote FTP server.
		/// </summary>
		/// <param name="fileName"></param>
		public void DeleteFile(string fileName)
		{
			if ( !this.loggedin ) 
			{
				this.Login();
			}

			/// <summary>
			/// Causes the file specified in the pathname to be deleted at the server site
			/// </summary>
			/// <remarks>Reply codes: 250 450 550 500 501 502 421 530</remarks>
			this.sendCommand( "DELE " + fileName );

			if ( this.resultCode != 250 )
			{
//				FireException(this.result.Substring(4));
				FireException(this.result);
			}
		}
		#endregion

		//4.修改文件名
		#region 重命名FTP上的某个文件
		/// <summary>
		/// Rename a file on the remote FTP server.
		/// </summary>
		/// <param name="oldFileName"></param>
		/// <param name="newFileName"></param>
		/// <param name="overwrite">setting to false will throw exception if it exists</param>
		public void RenameFile(string oldFileName,string newFileName, bool overwrite)
		{
			if ( !this.loggedin )
			{
				this.Login();
			}

			/// <summary>
			/// Specifies the old pathname of the file which is to be renamed. This 
			/// command must be immediately followed by a "rename to" command specifying the new 
			/// file pathname.
			/// </summary>
			/// <remarks>Reply codes: 450 550 500 501 502 421 530 350</remarks>
			this.sendCommand( "RNFR " + oldFileName );

			if ( this.resultCode != 350 ) 
			{
//				FireException(this.result.Substring(4));
				FireException(this.result);
			}

//			if ( !overwrite && this.GetFileList(newFileName).Length > 0 ) 
//				FireException("File already exists");

			// verify if newFileName exists
			if ( !overwrite)
			{
				this.sendCommand("SIZE " + newFileName);
				if ( this.resultCode == 213 )
					FireException("File already exists");
			}

			/// <summary>
			/// Specifies the new pathname of the file specified in the immediately 
			/// preceding "rename from" command. Together the two commands cause a file to be renamed.
			/// </summary>
			/// <remarks>Reply codes: 250 532 553 500 501 502 503 421 530</remarks>
			this.sendCommand( "RNTO " + newFileName );

			if ( this.resultCode != 250 ) 
			{
//				FireException(this.result.Substring(4));
				FireException(this.result);
			}
        }
		#endregion
//-----------------------------------------------------------------------------//





//-----------------------------------------------------------------------------//
		//PARTII:文件夹操作

		#region 在FTP上创建一个文件夹
		/// <summary>
		/// Create a directory on the remote FTP server.
		/// </summary>
		/// <param name="dirName"></param>
		public void MakeDir(string dirName)
		{
			if (!this.loggedin ) 
			{
				this.Login();
			}

			/// <summary>
			/// Causes the directory specified in the pathname to be created as 
			/// a directory (if the pathname is absolute) or as a subdirectory of the current 
			/// working directory (if the pathname is relative).
			/// </summary>
			/// <remarks>Reply codes: 257 500 501 502 421 530 550</remarks>
			this.sendCommand( "MKD " + dirName );

			if ( this.resultCode != 250 && this.resultCode != 257 ) 
			{
//				FireException(this.result.Substring(4));
				FireException(this.result);
			}
		}
		#endregion

		#region 删除FTP上的一个文件夹
		/// <summary>
		/// Delete a directory on the remote FTP server.
		/// </summary>
		/// <param name="dirName"></param>
		public void RemoveDir(string dirName)
		{
			if ( !this.loggedin ) 
			{
				this.Login();
			}

			/// <summary>
			/// Causes the directory specified in the pathname to be removed as 
			/// a directory (if the pathname is absolute) or as a subdirectory of the current 
			/// working directory (if the pathname is relative).
			/// </summary>
			/// <remarks>Reply codes: 250 500 501 502 421 530 550</remarks>
			this.sendCommand( "RMD " + dirName );

			if ( this.resultCode != 250 )
			{
//				FireException(this.result.Substring(4));
				FireException(this.result);
			}
		}
		#endregion

		#region 改变客户端目前浏览到的FTP上的目录（文件夹层次）
		/// <summary>
		/// Change the current working directory on the remote FTP server.
		/// </summary>
		/// <param name="dirName"></param>
		public bool ChangeDir(string dirName)
		{
			if( dirName == null || dirName.Equals(".") || dirName.Length == 0 )
			{
				return false;
			}

			if ( !this.loggedin )
			{
				this.Login();
			}

			/// <summary>
			/// Changes the current directory
			/// </summary>
			/// <remarks>Reply codes: 250 500 501 502 421 530 550</remarks>
			this.sendCommand( "CWD " + dirName );

			//if ( ( this.resultCode != 250 ) && ( this.resultCode != 150 ) )

			if ( (this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 530) || (this.resultCode == 550) )
			{
//				FireException(result.Substring(4));
				FireException(this.result);
				return false;
			}

			/// <summary>
			/// Causes the name of the current working directory to be returned in the reply.
			/// </summary>
			/// <remarks>Reply codes: 257 500 501 502 421 550</remarks>
			this.sendCommand( "PWD" );

			if ( (this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 550) )
			{
//				FireException(result.Substring(4));
				FireException(this.result);
				return false;
			}

			//有时候message不包含","
			return true;

			// gonna have to do better than this....
//			this.remotePath = this.message.Split('"')[1];
//
//			showMessage( "Current directory is " + this.remotePath, false );
//			return this.remotePath;
		}
		#endregion
//-----------------------------------------------------------------------------//





//-----------------------------------------------------------------------------//	
		//PARTIII:查看信息
		//1.查看文件夹
		#region 获取当前登录目录下的所有文件列表
		public string[] GetFileList()
		{
			return this.GetFileList("*.*");
		}
		#endregion

		#region 列出FTP下目录下的所有文件列表
		public string[] GetFileList(string mask)
		{
			if ( !this.loggedin )
			{
				this.Login();
			}

			Socket cSocket = createDataSocket();

			/// <summary>
			/// Causes a list to be sent from the server to the passive DTP. If 
			/// the pathname specifies a directory or other group of files, the server should 
			/// transfer a list of files in the specified directory. If the pathname specifies 
			/// a file then the server should send current information on the file. A null 
			/// argument implies the user's current working or default directory.
			/// </summary>
			/// <remarks>Reply codes: 125 150 226 250 425 426 451 450 500 501 502 421 530</remarks>
			this.sendCommand("LIST " + mask);

			//150 文件状态良好，打开数据连接   125  数据连接已打开，准备传送 
			//250 请求的文件操作完成           226  关闭数据连接，请求的文件操作成功
			if(!(this.resultCode == 150 || this.resultCode == 125 || this.resultCode == 250 || this.resultCode == 226)) 
			{
//				FireException(this.result.Substring(4));
				FireException(this.result);
			}

			this.message = "";

			string l_strOutput = "",l_strTemp = "";
			int l_iRetval = 0;

			receiveBuffer.Initialize();
			l_strTemp = "";
			l_strOutput = "";

			Thread.Sleep(700);

			for ( ; ( l_iRetval = cSocket.Receive(receiveBuffer)) > 0 ;  ) 
			{
				l_strTemp = Encoding.Default.GetString(receiveBuffer,0,l_iRetval);
				l_strOutput += l_strTemp;
				if ( cSocket.Available == 0 )
				{
					break;
				}
			}

			this.message = l_strOutput;
			string[] msg = this.message.Replace("\r","").Split('\n');

			cSocket.Close();

			if ( this.message.IndexOf( "No such file or directory" ) != -1 )
				msg = new string[]{};

			//			this.readResponse();
			//
			//			if ( this.resultCode != 226 )
			//				msg = new string[]{};
			//			//	FireException(result.Substring(4));

			return msg;
		}
		#endregion	
		

		//2.查看文件
		#region 获取FTP目录下的文件大小
		public long GetFileSize(string fileName)
		{
			if ( !this.loggedin ) 
			{
				this.Login();
			}

			this.sendCommand("SIZE " + fileName);
			long size=0;

			if ( this.resultCode == 213 )
				size = long.Parse(this.result.Substring(4));

			else
				FireException(this.result.Substring(4));

			return size;
		}
		#endregion
		
		#region 获取文件的修改日期
		public string GetModDate(string FileName)
		{
			if (!this.loggedin )
			{
				this.Login();
			}

			/// <summary>
			/// Changes the current directory
			/// </summary>
			/// <remarks>Reply codes: 250 500 501 502 421 530 550</remarks>
			this.sendCommand( "MDTM " + FileName );
			if(resultCode!=213)
			{
				return null;
			}
			string szTime = message;
			szTime = message.Remove(0,4);
			szTime = szTime.Substring(0,14);
			return szTime;
		}
		#endregion
//-----------------------------------------------------------------------------//
#endregion			


#region 想服务器发送ftp命令
        public bool QuoteFileType(string szFileType)
        {
            if (!this.loggedin)
            {
                this.Login();
            }

            this.sendCommand("site filetype=" + szFileType);

            if (this.resultCode == 200)
                return true;

            else
                FireException(this.result.Substring(4));

            return false;
        }
#endregion
//-----------------------------------------------------------------------------//
		/// PARTVI：发送FTP命令
		//发送FTP命令以及读取返回信息
		#region  读取向FTP服务器发送FTP命令后的返回码
		private void readResponse()
		{
			this.message = "";
			this.result = this.readLine();

			if ( this.result.Length > 3 )
				this.resultCode = int.Parse( this.result.Substring(0,3) );
			else
			{
				this.result = null;
				resultCode = 0;
			}
		}
		#endregion

		#region 读取向FTP服务器发送FTP命令后的反馈信息
		private string readLine()
		{
			string l_strOutput = "",l_strTemp = "";
			int l_iRetval = 0;

			receiveBuffer.Initialize();
			l_strTemp = "";
			l_strOutput = "";

			Thread.Sleep(600);

			/* Receive data in a loop until the FTP server sends it */
			//旧版本
//			for ( ; ( l_iRetval = clientSocket.Receive(receiveBuffer)) > 0 ;  ) 
//			{
//				l_strTemp = Encoding.Default.GetString(receiveBuffer,0,l_iRetval);
//				l_strOutput += l_strTemp;
//				if ( clientSocket.Available == 0 )
//				{
//					break;
//				}
//			}

			//新版本
//			while(clientSocket.Available>0 && ( l_iRetval = clientSocket.Receive(receiveBuffer)) > 0 ) 
//			{
//				l_strTemp = Encoding.Default.GetString(receiveBuffer,0,l_iRetval);
//				l_strOutput += l_strTemp;
//			}
			//新版本2	2007-08-29
			while(true)
			{
				l_iRetval = clientSocket.Receive(receiveBuffer, receiveBuffer.Length, 0);
				l_strTemp += Encoding.ASCII.GetString(receiveBuffer, 0, l_iRetval);
				l_strOutput += l_strTemp;
				if(l_iRetval < receiveBuffer.Length)
				{
					break;
				}
			}

			this.message = l_strOutput;

			string[] msg = this.message.Split('\n');

			if ( this.message.Length > 2 )
			{
				this.message = msg[ msg.Length - 2 ];
			}
			else
			{
				this.message = msg[0];
			}

			//如果没有接受到返回码，延迟700毫秒，重新进行读取返回码。返回码一般放在返回信息的头部？
			if ( this.message.Length >= 4 && !this.message.Substring(3,1).Equals(" ") ) 
			{
				return this.readLine();
			}

			return message;
		}
		#endregion

		#region 向FTP服务器发送命令
		private void sendCommand(string command)
		{
			int l_iRetval = 0;
			try
			{
				command = command.Trim()+ "\r\n";
              
				Byte[] cmdBytes = Encoding.ASCII.GetBytes(command .ToCharArray() );

				//发送消息
				l_iRetval = clientSocket.Send( cmdBytes, cmdBytes.Length, 0);
				this.readResponse();
			}
			catch
			{
                throw;
			}
		}
		#endregion
//-----------------------------------------------------------------------------//



//-----------------------------------------------------------------------------//
		//PARTV:建立数据传输的Socket
		#region 为了传输数据而单独开辟的Socket通道
		/// <summary>
		/// when doing data transfers, we need to open another socket for it.
		/// </summary>
		/// <returns>Connected socket</returns>
		private Socket createDataSocket()
		{
			/// <summary>
			/// Requests the server-DTP to "listen" on a data port (which is not
			/// its default data port) and to wait for a connection rather than initiate one
			/// upon receipt of a transfer command. The response to this command includes the 
			/// host and port address this server is listening on. 
			/// </summary>
			/// <remarks>Reply codes: 227 500 501 502 421 530</remarks>
			this.sendCommand("PASV");

			//227 进入被动模式
			if ( this.resultCode != 227 ) 
			{
//				FireException(this.result.Substring(4));
				FireException(this.result);
			}

			int index1 = this.result.IndexOf('(');
			int index2 = this.result.IndexOf(')');

			string ipData = this.result.Substring(index1+1,index2-index1-1);

			int[] parts = new int[6];

			int len = ipData.Length;
			int partCount = 0;
			string buf="";

			for (int i = 0; i < len && partCount <= 6; i++)
			{
				char ch = char.Parse( ipData.Substring(i,1) );

				//IsDigit（）判断一个数字是否为字符
				if ( char.IsDigit(ch) )
					buf+=ch;

				else if (ch != ',')
					FireException("Malformed PASV result: " + result);

				if ( ch == ',' || i+1 == len )
				{
					try
					{
						parts[partCount++] = int.Parse(buf);
						buf = "";
					}
					catch (Exception ex)
					{
						FireException("Malformed PASV result (not supported?): " + this.result, ex);
					}
				}
			}

			string ipAddress = parts[0] + "."+ parts[1]+ "." + parts[2] + "." + parts[3];

			int port = (parts[4] << 8) + parts[5];

			Socket socket = null;
			IPEndPoint ep = null;

			try
			{
				socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
				ep = new IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);
				socket.Connect(ep);
			}
			catch(Exception ex)
			{
				// doubtfull....
				if ( socket != null && socket.Connected ) socket.Close();

				FireException("Can't connect to remote server", ex);
			}

			return socket;
		}
		#endregion
//-----------------------------------------------------------------------------//
		



		
//-----------------------------------------------------------------------------//
		//PATRTVI:释放资源部分
		#region 资源回收
		/// <summary>
		/// Always release those sockets.
		/// </summary>
		private void cleanup()
		{
			if ( this.clientSocket!=null )
			{
				this.clientSocket.Close();
				this.clientSocket = null;
			}
			this.loggedin = false;
		}
		#endregion

		#region 析构函数
		/// <summary>
		/// Destuctor
		/// </summary>
		~FtpClient()
		{
			this.cleanup();
		}
		#endregion
//-----------------------------------------------------------------------------//





//-----------------------------------------------------------------------------//
		//PARTVII:非常情况下发出异常
		#region 抛出异常
		private void FireException(string message,Exception innerException)
		{
			throw new FtpException(message, innerException);
		}

		private void FireException(string message)
		{
			throw new FtpException(message);
		}
		#endregion
	}
}