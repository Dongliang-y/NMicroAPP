/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2021/11/8 9:43:41                            */
/*==============================================================*/


drop table if exists sys_ORGPosition_Role;

drop table if exists sys_Rule_Data;

drop table if exists sys_country;

drop table if exists sys_duty;

drop table if exists sys_forbid_res;

drop table if exists sys_group;

drop table if exists sys_group_user;

drop table if exists sys_login_record;

drop index Index_SendTime on sys_message;

drop table if exists sys_message;

drop table if exists sys_organization;

drop table if exists sys_organization_position;

drop table if exists sys_position;

drop table if exists sys_position_user;

drop table if exists sys_recv_message;

drop table if exists sys_region;

drop index Index_resType on sys_resources;

drop table if exists sys_resources;

drop table if exists sys_role;

drop table if exists sys_role_group;

drop table if exists sys_role_position;

drop table if exists sys_role_resources;

drop table if exists sys_role_type;

drop table if exists sys_unit;

drop index Index_login_name on sys_user;

drop table if exists sys_user;

drop table if exists sys_user_duty;

drop table if exists sys_user_resources;

drop table if exists sys_user_role;

/*==============================================================*/
/* Table: sys_ORGPosition_Role                                  */
/*==============================================================*/
create table sys_ORGPosition_Role
(
   id                   varchar(32) not null,
   PositionId           varchar(32) comment '机构岗位ID',
   RoleId               varchar(32) comment '角色ID',
   create_time          datetime comment '创建时间',
   create_user          national varchar(50) comment '创建人',
   primary key (id)
);

/*==============================================================*/
/* Table: sys_Rule_Data                                         */
/*==============================================================*/
create table sys_Rule_Data
(
   ID                   varchar(50) not null comment 'ID',
   DataName             varchar(200) comment '数据源名称',
   DataScript           varchar(100) comment '数据筛选脚本',
   RoleId               varchar(100) comment '角色ID',
   tabName              varchar(200) comment '关联表',
   primary key (ID)
);

alter table sys_Rule_Data comment 'sys_Rule_Data';

/*==============================================================*/
/* Table: sys_country                                           */
/*==============================================================*/
create table sys_country
(
   id                   national varchar(32) not null comment 'ID',
   code                 national varchar(50) comment '国家编码',
   all_name             national varchar(200) comment '国家全称',
   name                 national varchar(200) comment '国家名称',
   type                 national varchar(200) comment '国家分类（DICT）',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   sorted_num           int comment '排序号',
   dnsname              national varchar(200) comment '国际域名缩写',
   time_diff            national varchar(200) comment '时差',
   primary key (id)
);

alter table sys_country comment '国家';

/*==============================================================*/
/* Table: sys_duty                                              */
/*==============================================================*/
create table sys_duty
(
   id                   national varchar(50) not null comment '编码',
   name                 national varchar(500) comment '名称',
   descrption           nvarchar(2000) comment '描述',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   sorted_num           int comment '排序号',
   primary key (id)
);

alter table sys_duty comment '职务';

/*==============================================================*/
/* Table: sys_forbid_res                                        */
/*==============================================================*/
create table sys_forbid_res
(
   id                   national varchar(32) not null comment '编号',
   resources_id         national varchar(50) comment '资源id',
   user_id              national varchar(50) comment '用户id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_forbid_res comment '禁止资源';

/*==============================================================*/
/* Table: sys_group                                             */
/*==============================================================*/
create table sys_group
(
   id                   national varchar(50) not null comment '编码',
   name                 national varchar(500) comment '名称',
   descrption           national varchar(2000) comment '描述',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_group comment '用户组';

/*==============================================================*/
/* Table: sys_group_user                                        */
/*==============================================================*/
create table sys_group_user
(
   id                   national varchar(32) not null comment '编码',
   user_id              national varchar(50) comment '用户id',
   user_group_id        national varchar(50) comment '用户组Id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_group_user comment '用户组-用户';

/*==============================================================*/
/* Table: sys_login_record                                      */
/*==============================================================*/
create table sys_login_record
(
   id                   national varchar(32) not null comment '编码',
   contents             national varchar(500) comment '内容',
   user_Id              national varchar(50) comment '用户id',
   ip                   national varchar(50) comment '登录IP',
   address              national varchar(250) comment '登录地点',
   login_time           datetime comment '登录时间',
   primary key (id)
);

alter table sys_login_record comment '用户登录记录';

/*==============================================================*/
/* Table: sys_message                                           */
/*==============================================================*/
create table sys_message
(
   id                   nvarchar(32) not null comment '消息主键',
   user_id              nvarchar(100) comment '发送用户ID',
   Contents             text comment '消息内容',
   send_time            datetime comment '发送时间',
   type                 nvarchar(100) comment '消息类型(DICT)',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_message comment '消息表';

/*==============================================================*/
/* Index: Index_SendTime                                        */
/*==============================================================*/
create index Index_SendTime on sys_message
(
   send_time
);

/*==============================================================*/
/* Table: sys_organization                                      */
/*==============================================================*/
create table sys_organization
(
   id                   national varchar(50) not null comment '机构编码',
   name                 national varchar(1000) comment '机构名称',
   short_name           national varchar(1000) comment '机构简称',
   english_name         national varchar(1000) comment '机构英文名称',
   pingyin              national varchar(1000) comment '机构全拼名称',
   short_pingyin        national varchar(1000) comment '机构简拼名称',
   multi_org_type       national varchar(100) comment '多机构类型(DICT)',
   organization_type    national varchar(100) comment '机构组织类型(DICT)',
   buz_type             national varchar(100) comment '机构业务类型(DICT)',
   address              national varchar(500) comment '机构所在地点',
   virtualed            int comment '是否虑机构',
   level_code           national varchar(500) comment '机构层级编码',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   sorted_num           int comment '排序号',
   principal            national varchar(500) comment '第一负责人',
   parent_id            national varchar(50) comment '父机构ID',
   primary key (id)
);

alter table sys_organization comment '机构';

/*==============================================================*/
/* Table: sys_organization_position                             */
/*==============================================================*/
create table sys_organization_position
(
   id                   nvarchar(32) not null comment '编码',
   organization_id      national varchar(50) comment '机构编码',
   name                 national varchar(500) comment '岗位名称',
   descrption           nvarchar(2000) comment '描述',
   primary key (id)
);

alter table sys_organization_position comment '机构岗位设定';

/*==============================================================*/
/* Table: sys_position                                          */
/*==============================================================*/
create table sys_position
(
   id                   nvarchar(32) not null comment '编码',
   descrption           nvarchar(2000) comment '描述',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   name                 national varchar(500) comment '岗位名称',
   primary key (id)
);

alter table sys_position comment '岗位';

/*==============================================================*/
/* Table: sys_position_user                                     */
/*==============================================================*/
create table sys_position_user
(
   id                   national varchar(50) not null comment '编码',
   position_id          national varchar(50) comment '岗位Id',
   user_id              national varchar(50) comment '用户id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_position_user comment '岗位-用户';

/*==============================================================*/
/* Table: sys_recv_message                                      */
/*==============================================================*/
create table sys_recv_message
(
   id                   national varchar(32) not null comment '主键',
   message_id           national varchar(32) comment '站内信ID',
   user_id              national varchar(32) comment '用户ID',
   read_time            datetime comment '阅读时间',
   state                national varchar(100) comment '阅读状态(DICT)',
   logical_delete       int comment '逻辑删除',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_recv_message comment '接收消息';

/*==============================================================*/
/* Table: sys_region                                            */
/*==============================================================*/
create table sys_region
(
   id                   national varchar(50) not null comment '区划id',
   name                 national varchar(200) comment '名称',
   short_name           national varchar(200) comment '简称',
   all_name             national varchar(200) comment '全称',
   at_level             int comment '等级',
   code                 national varchar(50) comment '编码',
   parent_id            national varchar(50) comment '上级区划Id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   country_id           national varchar(32) comment '所在国家Id',
   primary key (id)
);

alter table sys_region comment '行政区划';

/*==============================================================*/
/* Table: sys_resources                                         */
/*==============================================================*/
create table sys_resources
(
   id                   national varchar(50) not null comment '编码',
   name                 national varchar(500) comment '资源名称',
   alias_name           national varchar(500) comment '资源别名',
   res_type_code        national varchar(50) comment '资源类型（DICT)',
   res_addr_type        national varchar(500) comment '资源所在分类（DICT)',
   system_id            national varchar(50) comment '所在业务系统ID',
   icon_path            national varchar(500) comment '图标路径',
   path                 national varchar(500) comment '资源路径',
   description          national varchar(1500) comment '描述',
   level_code           national varchar(500) comment '资源层级编码',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   sorted_num           int comment '排序号',
   parent_id            national varchar(50) comment '父资源ID',
   primary key (id)
);

alter table sys_resources comment '资源';

/*==============================================================*/
/* Index: Index_resType                                         */
/*==============================================================*/
create index Index_resType on sys_resources
(
   res_type_code
);

/*==============================================================*/
/* Table: sys_role                                              */
/*==============================================================*/
create table sys_role
(
   id                   national varchar(50) not null comment '编码',
   name                 national varchar(500) comment '名称',
   role_type_id         national varchar(50) comment '角色分类id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   sorted_num           int comment '排序号',
   primary key (id)
);

alter table sys_role comment '角色';

/*==============================================================*/
/* Table: sys_role_group                                        */
/*==============================================================*/
create table sys_role_group
(
   id                   national varchar(32) not null comment '编码',
   role_id              national varchar(50) comment '角色ID',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   group_id             national varchar(50) comment '用户组ID',
   primary key (id)
);

alter table sys_role_group comment '角色-用户组';

/*==============================================================*/
/* Table: sys_role_position                                     */
/*==============================================================*/
create table sys_role_position
(
   id                   national varchar(32) not null comment '编码',
   role_id              national varchar(50) comment '角色ID',
   position_id          national varchar(50) comment '岗位Id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(50) comment '创建人',
   primary key (id)
);

alter table sys_role_position comment '角色_ 岗位';

/*==============================================================*/
/* Table: sys_role_resources                                    */
/*==============================================================*/
create table sys_role_resources
(
   id                   national varchar(32) not null comment '编码',
   role_id              national varchar(50) comment '角色Id',
   resources_id         national varchar(50) comment '资源Id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_role_resources comment '角色-资源';

/*==============================================================*/
/* Table: sys_role_type                                         */
/*==============================================================*/
create table sys_role_type
(
   id                   national varchar(50) not null comment '编码',
   name                 national varchar(50) comment '分类名称',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_role_type comment '角色分类';

/*==============================================================*/
/* Table: sys_unit                                              */
/*==============================================================*/
create table sys_unit
(
   id                   national varchar(50) not null comment '单位编码',
   description          national varchar(1500) comment '描述',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_unit comment '单位';

/*==============================================================*/
/* Table: sys_user                                              */
/*==============================================================*/
create table sys_user
(
   id                   national varchar(100) not null comment '员工编码',
   org_id               national varchar(50) comment '机构编码Id',
   region_id            national varchar(50) comment '所在区划id',
   name                 national varchar(50) comment '姓名',
   nick_name            national varchar(50) comment '昵称',
   login_name           national varchar(50) comment '登录名',
   password             national varchar(50) comment '登录密码',
   pingyin_name         national varchar(150) comment '姓名拼音',
   head_icon            national varchar(150) comment '头像(附件的Id)',
   native_place         national varchar(150) comment '籍贯',
   national             national varchar(50) comment '民族(DICT)',
   card_num             national varchar(50) comment '身份证',
   mobile_num           national varchar(50) comment '手机号码',
   office_tel_num       national varchar(50) comment '办公电话',
   email                national varchar(50) comment '工作邮箱',
   weixin               national varchar(250) comment '微信号',
   qq                   national varchar(50) comment 'QQ号',
   work_address         national varchar(500) comment '工作地址',
   sex                  national varchar(50) comment '性别(DICT)',
   birthday             date comment '出生年月',
   logic_delete         int comment '逻辑删除',
   descriptions         national varchar(2000) comment '个人介绍',
   regist_way           national varchar(150) comment '注册方式(DICT)',
   regist_time          datetime comment '注册时间',
   regist_device        national varchar(250) comment '注册设备',
   last_login_time      national varchar(250) comment '上次登录时间',
   user_state           national varchar(250) comment '用户状态(DICT)',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   system_id            national varchar(150) comment '业务ID',
   sorted_num           int comment '排序号',
   app_token            national varchar(250) comment 'AppToken',
   last_login_device    national varchar(250) comment '上次登录设备',
   primary key (id)
);

alter table sys_user comment '用户';

/*==============================================================*/
/* Index: Index_login_name                                      */
/*==============================================================*/
create index Index_login_name on sys_user
(
   login_name
);

/*==============================================================*/
/* Table: sys_user_duty                                         */
/*==============================================================*/
create table sys_user_duty
(
   id                   national varchar(32) not null comment '编号',
   user_id              national varchar(100) comment '员工编码',
   duty_id              national varchar(50) comment '职务编码',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_user_duty comment '用户-职务';

/*==============================================================*/
/* Table: sys_user_resources                                    */
/*==============================================================*/
create table sys_user_resources
(
   id                   national varchar(32) not null comment '编号',
   user_id              national varchar(50) comment '用户Id',
   resource_id          national varchar(50) comment '资源Id',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   primary key (id)
);

alter table sys_user_resources comment '用户-资源';

/*==============================================================*/
/* Table: sys_user_role                                         */
/*==============================================================*/
create table sys_user_role
(
   id                   national varchar(32) not null comment '编码',
   role_id              national varchar(50) comment '角色ID',
   create_time          datetime comment '创建时间',
   create_user          national varchar(500) comment '创建人',
   user_id              national varchar(50) comment '用户Id',
   primary key (id)
);

alter table sys_user_role comment '用户-角色';

alter table sys_ORGPosition_Role add constraint FK_Reference_33 foreign key (RoleId)
      references sys_role (id) on delete restrict on update restrict;

alter table sys_ORGPosition_Role add constraint FK_Reference_34 foreign key (PositionId)
      references sys_organization_position (id) on delete restrict on update restrict;

alter table sys_forbid_res add constraint FK_Reference_7 foreign key (resources_id)
      references sys_resources (id) on delete restrict on update restrict;

alter table sys_forbid_res add constraint FK_Reference_8 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_group_user add constraint FK_Reference_5 foreign key (user_group_id)
      references sys_group (id) on delete restrict on update restrict;

alter table sys_group_user add constraint FK_Reference_6 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_login_record add constraint FK_Reference_26 foreign key (user_Id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_message add constraint FK_Reference_29 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_organization_position add constraint FK_Reference_27 foreign key (organization_id)
      references sys_organization (id) on delete restrict on update restrict;

alter table sys_position add constraint FK_Reference_32 foreign key (id)
      references sys_organization_position (id) on delete restrict on update restrict;

alter table sys_position_user add constraint FK_Reference_11 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_position_user add constraint FK_Reference_12 foreign key (position_id)
      references sys_position (id) on delete restrict on update restrict;

alter table sys_recv_message add constraint FK_Reference_30 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_recv_message add constraint FK_Reference_31 foreign key (message_id)
      references sys_message (id) on delete restrict on update restrict;

alter table sys_region add constraint FK_Reference_region_country foreign key (country_id)
      references sys_country (id) on delete restrict on update restrict;

alter table sys_role add constraint FK_Reference_9 foreign key (role_type_id)
      references sys_role_type (id) on delete restrict on update restrict;

alter table sys_role_group add constraint FK_Reference_23 foreign key (group_id)
      references sys_group (id) on delete restrict on update restrict;

alter table sys_role_group add constraint FK_Reference_24 foreign key (role_id)
      references sys_role (id) on delete restrict on update restrict;

alter table sys_role_position add constraint FK_Reference_17 foreign key (position_id)
      references sys_position (id) on delete restrict on update restrict;

alter table sys_role_position add constraint FK_Reference_18 foreign key (role_id)
      references sys_role (id) on delete restrict on update restrict;

alter table sys_role_resources add constraint FK_Reference_13 foreign key (resources_id)
      references sys_resources (id) on delete restrict on update restrict;

alter table sys_role_resources add constraint FK_Reference_14 foreign key (role_id)
      references sys_role (id) on delete restrict on update restrict;

alter table sys_user add constraint FK_Reference_10 foreign key (org_id)
      references sys_organization (id) on delete restrict on update restrict;

alter table sys_user add constraint FK_Reference_28 foreign key (region_id)
      references sys_region (id) on delete restrict on update restrict;

alter table sys_user_duty add constraint FK_Reference_3 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_user_duty add constraint FK_Reference_4 foreign key (duty_id)
      references sys_duty (id) on delete restrict on update restrict;

alter table sys_user_resources add constraint FK_Reference_15 foreign key (resource_id)
      references sys_resources (id) on delete restrict on update restrict;

alter table sys_user_resources add constraint FK_Reference_16 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_user_role add constraint FK_Reference_21 foreign key (user_id)
      references sys_user (id) on delete restrict on update restrict;

alter table sys_user_role add constraint FK_Reference_22 foreign key (role_id)
      references sys_role (id) on delete restrict on update restrict;

