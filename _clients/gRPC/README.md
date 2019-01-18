# gRPC Clients

## Account Manager
The Account Manager creates accounts, retrieves a list of the new accounts, then loops through and closes each one. Each step of the process showcasing gRPC service-to-service communications to CoreServices.

## Background Worker
The Background Worker polls a message queue waiting for tasks to act upon. It uses an exponential backoff strategy to lower amount of polling during times when the system has less activity. 

## Custodial Processor
The Custodial Processor runs at a set interval and handles tasks such as garbage collection, expired data migration, and other such tasks.
