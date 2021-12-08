Publisher has 4 types of sensors:
- air temperature (air_temp)
- substrate temperature (sub_temp)
- air humidity (air_hum)
- substrate humidity (sub_hum)

Each type of sensor has 30 sensors.

Sensors are in tomato greenhouse.
They check, if conditions are perfect to grow tomatoes.
Firstly, each sensor value is randomed from a specified range.
Then they are changed by -1, 0 or 1 with probability.
Sensors queue is declared and binded with exchange.
Messages are published with id sensor, sensor value and datetime.

