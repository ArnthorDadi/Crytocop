import pika
import requests
import json

credentials = pika.PlainCredentials('yqrbpfxq', 'DstEdXfIUOCHv2GzlvSbLRhbiym8RVTt')
parameters = pika.ConnectionParameters(host='bonobo.rmq.cloudamqp.com',
                                       port=5672,
                                       virtual_host='yqrbpfxq',
                                       credentials=credentials)
connection = pika.BlockingConnection(parameters)
channel = connection.channel()

exchange_name = 'Cryptocop'
create_order_routing_key = 'create-order'
email_queue_name = 'email-queue'

email_template = '<h2>Thank you for ordering @ Cryptocop!</h2><p>We hope you will enjoy our lovely product and don\'t hesitate to contact us if there are any questions.</p><h3>Order</h3><table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Name of customer</th><th>Street name and number</th><th>City</th><th>Zip code</th><th>Country</th><th>Date of order</th><th>Total price</th></tr></thead><tbody>%s</tbody></table><br /><h3>Order Items</h3><table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>ProductIdentifier</th><th>Quantity</th><th>UnitPrice</th><th>TotalPrice</th></tr></thead><tbody>%s</tbody></table>'
                    
                    #'<h2>Thank you for ordering @ Cactus heaven!</h2>'+
                    #'<p>We hope you will enjoy our lovely product and don\'t hesitate to contact us if there are any questions.</p>'+
                    #'<h3>Order</h3>'+
                    #'<table>'+
                    #    '<thead>'+
                    #        '<tr style="background-color: rgba(155, 155, 155, .2)">'+
                    #            '<th>Name of customer</th>'+
                    #            '<th>Street name and number</th>'+
                    #            '<th>City</th>'+
                    #            '<th>Zip code</th>'+
                    #            '<th>Country</th>'+
                    #            '<th>Date of order</th>'+
                    #            '<th>Total price</th>'+
                    #        '</tr>'+
                    #    '</thead>'+
                    #    '<tbody>%s</tbody>'+
                    #'</table>'+
                    #'<h3>Order Items</h3>'+
                    #'<table>'+
                    #    '<thead>'+
                    #        '<tr style="background-color: rgba(155, 155, 155, .2)">'+
                    #            '<th>ProductIdentifier</th>'+
                    #            '<th>Quantity</th>'+
                    #            '<th>UnitPrice</th>'+
                    #            '<th>TotalPrice</th>'+
                    #        '</tr>'+
                    #    '</thead>'+
                    #    '<tbody>%s</tbody>'+
                    #'</table>'

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='direct', durable=False)
# Declare the queue, if it doesn't exist
channel.queue_declare(queue=email_queue_name, durable=True)
# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchange_name, queue=email_queue_name, routing_key=create_order_routing_key)

def send_simple_message(to, subject, body):
    return requests.post(
        "https://api.mailgun.net/v3/sandbox0f3ab2ab4e7a45b3ad60fe15ac533590.mailgun.org/messages",
        auth=("api", "5bdc7a00b5ca05d895b0fe1a17769dbe-53c13666-46a97655"),
        data={"from": "Mailgun Sandbox <postmaster@sandbox0f3ab2ab4e7a45b3ad60fe15ac533590.mailgun.org>",
              "to": to,
              "subject": subject,
              "html": body})

def send_order_email(ch, method, properties, data):
    parsed_msg = json.loads(data)

    Email = parsed_msg['Email']
    FullName = parsed_msg['FullName']
    StreetName = parsed_msg['StreetName']
    HouseNumber = parsed_msg['HouseNumber']
    ZipCode = parsed_msg['ZipCode']
    Country = parsed_msg['Country']
    City = parsed_msg['City']
    CardholderName = parsed_msg['CardholderName']
    CreditCard = parsed_msg['CreditCard']
    OrderDate = parsed_msg['OrderDate']
    TotalPrice = parsed_msg['TotalPrice']
    OrderItems = parsed_msg['OrderItems']

    print(OrderItems)
    
    #OrderDto:
    #{
    #    "Id":1,
    #    "Email":"Test@email.com",
    #    "FullName":"Test Testmundsson",
    #    "StreetName":"Testmund Street",
    #    "HouseNumber":"15",
    #    "ZipCode":"109",
    #    "Country":"TestCountry",
    #    "City":"TestCity",
    #    "CardholderName":"Test Testmundsson Card Name",
    #    "CreditCard":"************5555",
    #    "OrderDate":"13.11.2020",
    #    "TotalPrice":5.165,
    #    "OrderItems":[
    #        {
    #            "Id":1,
    #            "ProductIdentifier":"BTC",
    #            "Quantity":1.0,
    #            "UnitPrice":5.165,
    #            "TotalPrice":5.165,
    #            "Links":{}
    #        }
    #    ],
    #    "Links":{}
    #}

    order_html = ''.join('<tr><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%i</td></tr>' % ( FullName, StreetName + " " + HouseNumber, City, ZipCode, Country, OrderDate, TotalPrice ))

    order_items_html = ''.join([ '<tr><td>%s</td><td>%i</td><td>%i</td><td>%i</td></tr>' % (OrderItem['ProductIdentifier'], OrderItem['Quantity'], OrderItem['UnitPrice'], OrderItem['TotalPrice']) for OrderItem in OrderItems])

    representation = email_template % (order_html, order_items_html)
    send_simple_message(Email, 'Successful order!', representation)
    print("Sent Order Confirmation to " + Email)

channel.basic_consume(email_queue_name,
                      send_order_email,
                      True)

channel.start_consuming()
connection.close()
