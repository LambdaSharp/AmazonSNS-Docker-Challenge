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

## Level 1

Publish a SNS topic message on `VirtualVacuumRobot` to start cleaning in the [AWS console](https://us-east-1.console.aws.amazon.com/sns/v3/home?region=us-east-1#/topics). `{'action':'start'}`.

## Level 2

After a couple cleanings, the dustbin gets full!  Human intervention is needed.  Subscribe to the SNS topic: `VirtualVacuumRobot_DUSTBIN_FULL` for a SMS message.  Then you will want to reset the dustbin using the command: `{'action':'dustbin'}`

## Level 3

## Level 4

## Boss

## Clean up

Publish a SNS  message to clean up: `{'action': 'teardown'}`
