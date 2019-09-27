-- create DATABASE GameServerDB;

USE GameServerDB;

DROP TABLE IF EXISTS UserInfo;
CREATE TABLE IF NOT EXISTS UserInfo(
	id int(11) PRIMARY KEY auto_increment,
	username varchar(20) not null,
    password varchar(50) not null,
    registerDate date
);

INSERT INTO UserInfo(id,username,password)VALUES(1,'InnerAccount','zdstudio2019');