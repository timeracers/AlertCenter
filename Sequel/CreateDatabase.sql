CREATE SCHEMA alert;

DROP TABLE IF EXISTS alert."Topics" CASCADE;

CREATE TABLE alert."Topics" (
   name   text   PRIMARY KEY
);

DROP TABLE IF EXISTS alert."Alerts" CASCADE;

CREATE TABLE alert."Alerts"(
   topic   text   NOT NULL   REFERENCES alert."Topics" (name),
   userid   uuid   NOT NULL,
   username   text   NOT NULL,
   message   text   NOT NULL,
   unixepoch   bigint   NOT NULL,
   PRIMARY KEY (unixepoch, topic, userid, message)
);

DROP TABLE IF EXISTS alert."Emails" CASCADE;

CREATE TABLE alert."Emails"(
   userid   uuid   PRIMARY KEY,
   username text   NOT NULL   UNIQUE,
   email   text   NOT NULL
);

DROP TABLE IF EXISTS alert."Subscriptions" CASCADE;

CREATE TABLE alert."Subscriptions"(
   userid   uuid   NOT NULL   REFERENCES alert."Emails" (userid),
   topic   text   NOT NULL   REFERENCES alert."Topics" (name),
   PRIMARY KEY (userid, topic)
);
