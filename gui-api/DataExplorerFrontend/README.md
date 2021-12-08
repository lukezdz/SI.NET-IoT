# Greenhouse GUI

Available sensor types:
- Air temperature
- Air humidity
- Substrate temperature
- Substrate humidity

In greenhouse are placed 30 groups of all types of sensors in different places. 

## Features

**GUI has 4 types of tables, all handle generating CSV and showing chart. Types are pages in GUI also.**

Types below:

- Air Temperature
- Substrate temperature
- Air Humidity
- Substrate Humidity

Every table has name which is sensor type and 4 field:
- id -> int *{0<}* unique row identifier
- SensorId -> int *{1<= SensorId <=30}* number of sensor
- Value -> int *{1<x<100}* value of sensor
- Date -> dateTime *{dateType}* dateTime of received data from sensor


## Example chart

![alt text](https://i.imgur.com/P8wyWsY.png)
