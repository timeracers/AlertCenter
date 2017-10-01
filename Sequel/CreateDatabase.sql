/*Postgresql Setup*/

CREATE SCHEMA alert
GO

DROP TABLE IF EXISTS alert."Topics" CASCADE
GO

CREATE TABLE alert."Topics" (
   name   text   PRIMARY KEY
)
GO

DROP TABLE IF EXISTS alert."Alerts" CASCADE
GO

CREATE TABLE alert."Alerts"(
   topic   text   NOT NULL   REFERENCES alert."Topics" (name),
   userid   uuid   NOT NULL,
   username   text   NOT NULL,
   message   text   NOT NULL,
   unixepoch   bigint   NOT NULL,
   PRIMARY KEY (unixepoch, topic, userid, message)
)
GO

DROP TABLE IF EXISTS alert."Emails" CASCADE
GO

CREATE TABLE alert."Emails"(
   userid   uuid   PRIMARY KEY,
   username text   NOT NULL   UNIQUE,
   email   text   NOT NULL
)
GO

DROP TABLE IF EXISTS alert."Subscriptions" CASCADE
GO

CREATE TABLE alert."Subscriptions"(
   userid   uuid   NOT NULL   REFERENCES alert."Emails" (userid),
   topic   text   NOT NULL   REFERENCES alert."Topics" (name),
   PRIMARY KEY (userid, topic)
)
GO
