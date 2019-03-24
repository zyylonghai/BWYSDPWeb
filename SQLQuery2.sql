select request_session_id spid,OBJECT_NAME(resource_associated_entity_id) tableName ,*
from sys.dm_tran_locks where resource_type='OBJECT'

exec sp_lock


SELECT
request_session_id spid,
OBJECT_NAME(
resource_associated_entity_id
) tableName
FROM
sys.dm_tran_locks
WHERE
resource_type = 'OBJECT' 
ORDER BY request_session_id ASC
--spid 锁表进程 
--tableName 被锁表名

--根据锁表进程查询相应进程互锁的SQL语句
DBCC INPUTBUFFER (57)



Transaction (Process ID 55) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction.
EXEC sp_executesql N'update [temp] set fieldvalue=@fieldvalu,actions=@actions where sessionid=@sessionid and progid=@progid and tableNm=@tbnm and rowid=@rwindx and fieldnm=@fieldnm   ',N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu ntext,@actions smallint',@sessionid='gbxaj31pxzilj3vgf1wmj423',@progid='CheckBill',@tbnm='CheckBill',@rwindx=0,@fieldnm='BillNo',@fieldvalu=N'T201903160001',@actions=0  
EXEC sp_executesql N'update [temp] set fieldvalue=@fieldvalu,actions=@actions where sessionid=@sessionid and progid=@progid and tableNm=@tbnm and rowid=@rwindx and fieldnm=@fieldnm   ',N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu ntext,@actions smallint',@sessionid='gbxaj31pxzilj3vgf1wmj423',@progid='CheckBill',@tbnm='CheckBill',@rwindx=0,@fieldnm='Qty',@fieldvalu=N'110',@actions=0  