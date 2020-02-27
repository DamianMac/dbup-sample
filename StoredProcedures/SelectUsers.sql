IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SelectUsers')
BEGIN
    DROP PROCEDURE SelectUsers
END

GO

CREATE PROCEDURE SelectUsers

AS

BEGIN

    SELECT Id, UserName
    FROM
        Users

END

GO
