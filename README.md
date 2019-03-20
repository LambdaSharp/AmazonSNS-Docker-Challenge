# AmazonSNS-Docker-Challenge

In this challenge, we'll be exploring Amazon SNS for publishing and handling events.

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

<details><summary>Not hard enough for you?</summary>
Great! Can you do this in Fargate? Or EKS?
</details>

## Level 1

Publish a SNS topic message on `VirtualVacuumRobot` to start cleaning in the [AWS console](https://us-east-1.console.aws.amazon.com/sns/v3/home?region=us-east-1#/topics). `{'action':'start'}`.

<details><summary>Not hard enough for you?</summary>
Great! Can you send each vacuum to charge at 20% power?
</details>

## Level 2

After a couple cleanings, the dustbin gets full! Human intervention is needed. Subscribe to the SNS topic: `VirtualVacuumRobot_DUSTBIN_FULL` for a SMS message. Then you will want to reset the dustbin using the command: `{'action':'dustbin'}`

<details><summary>Not hard enough for you?</summary>
Great! Can you listen for this sns topic in lambda function and clear the dustbin via a sns message?
</details>

## Level 3

You're getting the hang of it! Although manually sending topic messages works in some cases it would be much nicer to schedule a cleaning job to start the vacuums. For level 3 go ahead and create a lambda function to kickoff a new cleaning job every 5 minutes.

<details><summary>Not hard enough for you?</summary>
Great! Make each vacuum clean one at a time but never the same one twice in a row.
</details>

## Level 4

All these messages are starting to get a bit overwhelming - let's add this data to a CloudWatch dashboard to draw insights from the virtual vacuum data. Each metric is a sum statistic every 30 seconds with a total period of 30 minutes. See example below.

![Cloudwatch example](dashboard.png)

## Boss
<img src="https://camo.githubusercontent.com/24ee58920381e83562f9780036a8df86ef9dec18/687474703a2f2f696d61676573322e66616e706f702e636f6d2f696d6167652f70686f746f732f31303430303030302f426f777365722d6e696e74656e646f2d76696c6c61696e732d31303430333230332d3530302d3431332e6a7067" alt="boss" data-canonical-src="http://images2.fanpop.com/image/photos/10400000/Bowser-nintendo-villains-10403203-500-413.jpg" style="max-width:100%;"></a></p>

Rarely (1/1000 chances) vacuums get stuck!  Can you please send yourself a SMS then shutdown the vacuum with: `{'action':'shutdown'}`

## Clean up

Publish a SNS message to clean up: `{'action': 'teardown'}`
