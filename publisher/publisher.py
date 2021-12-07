import random
import time
import pika
import numpy
from datetime import datetime


def main():
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq.local', port=5672))
    # connection = pika.BlockingConnection(pika.ConnectionParameters(host='localhost', port=5672))
    channel = connection.channel()

    # Greenhouse tomatoes environment (best conditions):
    # air temperature -> 22-27 degrees Celsius
    # substrate temperature -> 15-18 degrees Celsius
    # air humidity -> 60-65 %
    # substrate humidity  -> 70-85 %

    # Min max conditions values
    min_air_temp = 22
    max_air_temp = 27

    min_sub_temp = 15
    max_sub_temp = 18

    min_air_hum = 60
    max_air_hum = 65

    min_sub_hum = 70
    max_sub_hum = 85

    # Sensors dictionaries
    air_temp_dict = dict([(f'air_temp-{x + 1}', random.randrange(min_air_temp, max_air_temp)) for x in range(30)])
    sub_temp_dict = dict([(f'sub_temp-{x + 1}', random.randrange(min_sub_temp, max_sub_temp)) for x in range(30)])
    air_hum_dict = dict([(f'air_hum-{x + 1}', random.randrange(min_air_hum, max_air_hum)) for x in range(30)])
    sub_hum_dict = dict([(f'sub_hum-{x + 1}', random.randrange(min_sub_hum, max_sub_hum)) for x in range(30)])

    config = {
        'air_temp': air_temp_dict,
        'sub_temp': sub_temp_dict,
        'air_hum': air_hum_dict,
        'sub_hum': sub_hum_dict
    }
    print(config)

    while True:
        try:
            for x in air_temp_dict.keys():
                air_temp_diff = numpy.random.choice([-1, 0, 1], p=[0.2, 0.6, 0.2])
                air_temp_diff = air_temp_diff.item()
                air_temp_dict[x] = max(min_air_temp, air_temp_dict[x] + air_temp_diff)
                air_temp_dict[x] = min(max_air_temp, air_temp_dict[x] + air_temp_diff)

            for x in sub_temp_dict.keys():
                sub_temp_diff = numpy.random.choice([-1, 0, 1], p=[0.15, 0.7, 0.15])
                sub_temp_diff = sub_temp_diff.item()
                sub_temp_dict[x] = max(min_sub_temp, sub_temp_dict[x] + sub_temp_diff)
                sub_temp_dict[x] = min(max_sub_temp, sub_temp_dict[x] + sub_temp_diff)

            for x in air_hum_dict.keys():
                air_hum_diff = numpy.random.choice([-1, 0, 1], p=[0.2, 0.6, 0.2])
                air_hum_diff = air_hum_diff.item()
                air_hum_dict[x] = max(min_air_hum, air_hum_dict[x] + air_hum_diff)
                air_hum_dict[x] = min(max_air_hum, air_hum_dict[x] + air_hum_diff)

            for x in sub_hum_dict.keys():
                sub_hum_diff = numpy.random.choice([-1, 0, 1], p=[0.3, 0.4, 0.3])
                sub_hum_diff = sub_hum_diff.item()
                sub_hum_dict[x] = max(min_sub_hum, sub_hum_dict[x] + sub_hum_diff)
                sub_hum_dict[x] = min(max_sub_hum, sub_hum_dict[x] + sub_hum_diff)

            for x in air_temp_dict.keys():
                channel.basic_publish(exchange='',
                                      routing_key='sensors-queue',
                                      body=str.encode(
                                          str(datetime.now()) + ";" + str(x) + ";" + str(air_temp_dict[x])))  # all separated by ;
                print("sensors-queue: air temp message sent: " + x + " - " + str(air_temp_dict[x]))

            for x in sub_temp_dict.keys():
                channel.basic_publish(exchange='',
                                      routing_key='sensors-queue',
                                      body=str.encode(
                                          str(datetime.now()) + ";" + str(x) + ";" + str(sub_temp_dict[x])))  # all separated by ;                
                print("sensors-queue: sub temp message sent: " + x + " - " + str(sub_temp_dict[x]))
                time.sleep(0.001)

            for x in air_hum_dict.keys():
                channel.basic_publish(exchange='',
                                      routing_key='sensors-queue',
                                      body=str.encode(str(datetime.now()) + ";" + str(x) + ";" + str(
                                          air_hum_dict[x])))  # all separated by ;           
                print("sensors-queue: air hum message sent: " + x + " - " + str(air_hum_dict[x]))
                time.sleep(0.001)

            for x in sub_hum_dict.keys():
                channel.basic_publish(exchange='',
                                      routing_key='sensors-queue',
                                      body=str.encode(str(datetime.now()) + ";" + str(x) + ";" + str(sub_hum_dict[x]))) # all separated by ;
                print("sensors-queue: sub hum message sent: " + x + " - " + str(sub_hum_dict[x]))
                time.sleep(0.001)

        except KeyboardInterrupt:
            print('sensors-queue: closed')
            connection.close()
            break


if __name__ == "__main__":
    main()
