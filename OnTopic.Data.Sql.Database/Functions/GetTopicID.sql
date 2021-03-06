﻿--------------------------------------------------------------------------------------------------------------------------------
-- GET TOPIC ID
--------------------------------------------------------------------------------------------------------------------------------
-- Given a particular topic key, finds the FIRST instance of the TopicID associated with that key. Be aware that since keys are
-- not guaranteed to be unique, this may yield unexpected results if multiple topics share the same key; in that case, the first
-- key in the hierarchy will be returned.
--------------------------------------------------------------------------------------------------------------------------------

CREATE
FUNCTION	[dbo].[GetTopicID] (
	@TopicKey		NVARCHAR(255)
)
RETURNS	INT
AS

BEGIN

  ------------------------------------------------------------------------------------------------------------------------------
  -- DECLARE AND DEFINE VARIABLES
  ------------------------------------------------------------------------------------------------------------------------------
  DECLARE	@TopicID		INT	= -1

  ------------------------------------------------------------------------------------------------------------------------------
  -- GET TOPIC ID BASED ON TOPIC KEY
  ------------------------------------------------------------------------------------------------------------------------------
  SELECT	TOP 1
	@TopicID		= Topics.TopicID
  FROM	Attributes		Attributes
  JOIN	Topics		Topics
    ON	Attributes.TopicID	= Topics.TopicID
  WHERE	AttributeKey		= 'Key'
    AND	AttributeValue		= @TopicKey
  ORDER BY	RangeLeft		DESC
  OPTION (
    OPTIMIZE
    FOR (	@TopicKey		= 'Root'
    )
  )

  ------------------------------------------------------------------------------------------------------------------------------
  -- RETURN TOPIC ID
  ------------------------------------------------------------------------------------------------------------------------------
  RETURN	@TopicID

END