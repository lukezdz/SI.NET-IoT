# Greenhouse API

Available sensor types:
- Air temperature
- Air humidity
- Substrate temperature
- Substrate humidity

In greenhouse are placed 30 groups of all types of sensors in different places. 

## GET /api/{sensorType}

Available query params:
  - **page** -> int: number of page *{0<}*, if couldn't find specified page returns 404
  - **pageSize** -> int: quantity of data on page *{0<}*, if couldn't find any data on page returns 404
  
Available path params:
  - **sensorType** -> string: type of sensor *{air_temp, air_hum, sub_temp, sub_hum}*, if couldn't find returns 404
  


## GET /api/{sensorType}/{sensorId}

Available query params:
  - **page** -> int: number of page *{0<}*, if couldn't find specified page returns 404
  - **pageSize** -> int: quantity of data on page *{0<}*, if couldn't find any data on page returns 404
  
Available path params:
  - **sensorType** -> string: type of sensor *{air_temp, air_hum, sub_temp, sub_hum}*, if couldn't find returns 404
  - **sensorId** -> int: id of sensor *{1-30}*, if couldn't find returns 404
  
