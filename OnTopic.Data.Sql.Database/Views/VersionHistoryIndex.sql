﻿--------------------------------------------------------------------------------------------------------------------------------
-- VERSION HISTORY (INDEX)
--------------------------------------------------------------------------------------------------------------------------------
-- Creates a list of the last five versions for every topic in the database. This is useful for basic version rollbacks, by
-- providing a list of recent versions that can be reverted to.
--------------------------------------------------------------------------------------------------------------------------------
CREATE
VIEW	[dbo].[VersionHistoryIndex]
WITH	SCHEMABINDING
AS

WITH	TopicVersions
AS (
  SELECT	DISTINCT
	TopicID,
	Version,
	RowNumber		= ROW_NUMBER() OVER (
	  PARTITION BY		TopicID
	  ORDER BY		Version DESC
	)
  FROM	[dbo].[Attributes]
  GROUP BY	TopicID,
	Version
)
SELECT	Versions.TopicId,
	Version
FROM	TopicVersions		Versions
WHERE	RowNumber		<= 5