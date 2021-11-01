import random
import time

import pika
import argparse

# usage = '''====SENSORS=====
# Connect to remote rabbitmq host
# --user=guest --password=guest --host=192.168.1.200
# Specify exchange, automatically sets routing-key to blank
# --exchange=myexchange
# '''
# ap = argparse.ArgumentParser(description="RabbitMQ producer",
#                              epilog=usage,
#                              formatter_class=argparse.RawDescriptionHelpFormatter)
# ap.add_argument('--user', default="guest", help="user e.g. 'guest'")
# ap.add_argument('--password', default="guest", help="password e.g. 'pass'")
# ap.add_argument('--host', default="localhost", help="rabbitMQ host, defaults to localhost")
# ap.add_argument('--port', type=int, default=5672, help="rabbitMQ port, defaults to 5672")
# ap.add_argument('--exchange', default="", help="name of exchange to use, empty means default")
# ap.add_argument('--queue', default="sensors-queue", help="name of queue, defaults to 'sensors-queue'")
# ap.add_argument('--routing-key', default="sensors-queue", help="routing key, defaults to 'sensors-queue'")
# ap.add_argument('--body', default="test body", help="body of message, defaults to 'test body'")
# args = ap.parse_args()
# 
# credentials = pika.PlainCredentials(args.user, args.password)
# connection = pika.BlockingConnection(pika.ConnectionParameters(args.host, args.port, '/', credentials))
# channel = connection.channel()

connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq.com'))
channel = connection.channel()

while True:
    try:
        temp_dict = dict([(f'temp-{x + 1}', random.randrange(0, 60)) for x in range(30)])
        hum_dict = dict([(f'hum-{x + 1}', random.randrange(0, 100)) for x in range(30)])

        config = {
            'temp': temp_dict,
            'humidity': hum_dict
        }
        print(config)
        print(config['temp']['temp-5'])
        print(config['humidity']['hum-10'])

        for x in temp_dict.keys():
            channel.basic_publish(exchange='',
                                  routing_key='sensors-queue',
                                  body=str.encode(x))  # sensor's id
            channel.basic_publish(exchange='',
                                  routing_key='sensors-queue',
                                  body=temp_dict[x].to_bytes(1, 'little'))  # sensor's value
            print("sensors-queue: temp message sent")
            time.sleep(0.2)

        for x in hum_dict.keys():
            channel.basic_publish(exchange='',
                                  routing_key='sensors-queue',
                                  body=str.encode(x))  # sensor's id
            channel.basic_publish(exchange='',
                                  routing_key='sensors-queue',
                                  body=hum_dict[x].to_bytes(1, 'little'))  # sensor's value
            print("sensors-queue: humidity message sent")
            time.sleep(0.2)

    except KeyboardInterrupt:
        print('sensors-queue: closed')
        connection.close()
        break