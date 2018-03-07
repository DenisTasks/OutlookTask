-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetGroupChilds]
	@groupId int
AS	
BEGIN
	WITH C (GroupID, [GROUPNAME], ParentId, NESTINGLVL) AS
	(
		SELECT B.GroupId, B.[GroupName], B.ParentId, 1 FROM Groups AS B WHERE ParentId = @groupId
		UNION ALL
		SELECT B.GroupId, B.[GroupName], B.ParentId, (NESTINGLVL +1) FROM Groups AS B INNER JOIN C ON C.GroupID = B.ParentId 
	)
	SELECT GroupID FROM C	

END
