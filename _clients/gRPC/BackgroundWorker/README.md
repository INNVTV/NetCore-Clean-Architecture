# Background Worker

The Background Worker polls a message queue waiting for tasks to act upon. It uses an exponential backoff strategy to lower amount of polling during times when the system has less activity. 
