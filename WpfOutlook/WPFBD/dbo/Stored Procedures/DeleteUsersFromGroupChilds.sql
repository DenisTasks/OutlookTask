
CREATE PROCEDURE [dbo].[DeleteUsersFromGroupChilds]
	@groupId int,
	@List [dbo].[UserList] readonly
AS	
BEGIN
	WITH C (GroupID, [GROUPNAME], ParentId, NESTINGLVL) AS
	(
		SELECT B.GroupId, B.[GroupName], B.ParentId, 1 FROM Groups AS B WHERE ParentId = @groupId
		UNION ALL
		SELECT B.GroupId, B.[GroupName], B.ParentId, (NESTINGLVL +1) FROM Groups AS B INNER JOIN C ON C.GroupID = B.ParentId 
	)
	DELETE	FROM UserGroups 
			WHERE UserId IN (SELECT * FROM @List)
			AND GroupId IN (SELECT  GroupId  FROM  C )


END