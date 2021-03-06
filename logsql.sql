USE [BWYDB_Log]
GO
/****** Object:  StoredProcedure [dbo].[p_addlogM]    Script Date: 03/15/2020 13:37:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  alter proc [dbo].[p_addlogM]
  @logid nvarchar(50),
  @tablenm nvarchar(50),
  @ID bigint output,
  @logtbnm varchar(35) output
  as
  begin
     declare @minqty bigint
     
     insert into DataLogsM(LogId,TableNm) values(@logid,@tablenm)
     select top 1 @ID=ID From DataLogsM order by ID desc
     set @logtbnm='DataLogsD'+CONVERT(varchar(35),@ID%100)
     
	 if not exists(SELECT a.name FROM sysobjects AS a INNER JOIN sysindexes AS b ON a.id = b.id WHERE (a.type = 'u') AND (b.indid IN (0, 1)) and a.name=@logtbnm)
	 begin
	    exec('CREATE TABLE [dbo].'+@logtbnm+'(
                                    [ID] [bigint] NOT NULL ,
                                    [Action] [char](1) NOT NULL, 
                                    [UserId] [nvarchar](30) NOT NULL ,
                                    [IP] [nvarchar](15) NOT NULL ,
                                    [DT] [datetime] NOT NULL ,
                                    [FieldNm] [nvarchar](30) NOT NULL ,
                                    [FieldValue] [ntext] NULL,
                                    [OldFieldValue] [ntext] NULL,
                                    CONSTRAINT [PK_'+@logtbnm+'] PRIMARY KEY CLUSTERED (
                                    [ID] ASC,
                                    [Action] ASC,
                                    [UserId] ASC,
                                    [IP] ASC,
                                    [DT] ASC,
                                    [FieldNm] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')
	 end

     --declare @sqlstr varchar(1000)
     --set @sqlstr='insert into '+ @logtbnm+'(LogId,TableNm) values('''+@logid+''','''+@tablenm+''')'
     --EXEC(@sqlstr)
     --exec('select top 1 '''+@logtbnm+''' as logtbnm, ID from '+@logtbnm+' order by ID desc')
  end
  
  
  create proc [dbo].[p_GetlogM]
  @logid nvarchar(50),
  @tablenm nvarchar(50),
  @ID bigint output,
  @logtbnm varchar(35) output
  as
  begin
  select @ID=ID from DataLogsM where LogId=@logid and TableNm=@tablenm
  
  end
  
  
  
  
  begin
  declare @idtest bigint;
  declare @btnm varchar(35);
  set @IDtest=-1;
  EXEC dbo.[p_addlogM] @logid = N'00001', @tablenm = N'Account',@ID=@idtest output, @logtbnm=@btnm output
  SELECT @idtest,@btnm
  end
  
  
  
  
 declare @aa bigint
 set @aa=1
 while(@aa<10000000)
 begin
   insert into DataLogsM(LogId,TableNm) values('0001','account')
   set @aa=@aa+1
 end
 
 
 declare @idzyy bigint
 declare @tbzyy varchar(30)
 select top 1 @idzyy=ID From DataLogsM order by ID desc
 select @idzyy as 'aa',@tbzyy
 
 SELECT ID FROM DataLogsM WHERE LogId='a' and TableNm='b'