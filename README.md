# AmazonSNS-Docker-Challenge

In this challenge, we'll be exploring Amazon SNS for controlling applications using a message queues.

## Helpful Links

[.NET AWS SDK Docs](https://docs.aws.amazon.com/sdkfornet/v3/apidocs)

## Pre-requisites

The following tools and accounts are required to complete these instructions.

- [AWS Account](https://aws.amazon.com/)
- [Install AWS CLI](https://aws.amazon.com/cli/)
- [Install .NET Core 2.2](https://www.microsoft.com/net/download)
- [Docker](https://www.docker.com/get-started)

## Level 0

Start the docker containers locally. Follow the getting started guide here: https://github.com/skittleson/VirtualVacuumRobot#getting-started

Once the containers are ready, events like this will appear.

```bash
vvr_1  | {"id":5548,"message":"85","eventType":"READY","timestamp":"2019-03-20T17:58:36.0328565Z"}
vvr_2  | {"id":7766,"message":"88","eventType":"READY","timestamp":"2019-03-20T17:58:39.9083326Z"}
vvr_3  | {"id":412,"message":"83","eventType":"READY","timestamp":"2019-03-20T17:58:43.6412794Z"}
```

Publish SNS topic message to start cleaning.

## Level 1

## Level 2

## Level 3

## Level 4

## Boss

## Clean up

Publish a SNS  message to clean up: `{'action': 'teardown'}`
