const amqp = require('amqplib/callback_api');

const messageBrokerInfo = {
    exchanges: {
        order: 'Cryptocop'
    },
    queues: {
        orderQueue: 'payment-queue'
    },
    routingKeys: {
        createOrderKey: 'create-order'
    }
};

const createMessageBrokerConnection = () => new Promise((resolve, reject) => {
    amqp.connect('amqps://yqrbpfxq:DstEdXfIUOCHv2GzlvSbLRhbiym8RVTt@bonobo.rmq.cloudamqp.com/yqrbpfxq', function(err, conn) {
        if (err) { reject(err); }
        resolve(conn);
    });
});

const configureMessageBroker = channel => {
    const { exchanges, queues, routingKeys } = messageBrokerInfo;

    channel.assertExchange(exchanges.order, 'direct', { durable: false });
    channel.assertQueue(queues.orderQueue, { durable: true });
    console.log(" [*] Waiting for messages\nTo exit press CTRL+C");
    channel.bindQueue(queues.orderQueue, exchanges.order, routingKeys.createOrderKey);
}

const createChannel = connection => new Promise((resolve, reject) => {
    connection.createChannel((err, channel) => {
        if (err) { reject(err); }
        configureMessageBroker(channel);
        resolve(channel);
    });
});

(async () => {
    const connection = await createMessageBrokerConnection();
    const channel = await createChannel(connection);

    const { orderQueue } = messageBrokerInfo.queues;

    channel.consume(orderQueue, data => {
        /*
            OrderDto:
            {
                "Id":1,
                "Email":"Test@email.com",
                "FulEmaillName":"Test Testmundsson",
                "StreetName":"Testmund Street",
                "HouseNumber":"15",
                "ZipCode":"109",
                "Country":"TestCountry",
                "City":"TestCity",
                "CardholderName":"Test Testmundsson Card Name",
                "CreditCard":"************5555",
                "OrderDate":"13.11.2020",
                "TotalPrice":5.165,
                "OrderItems":[
                    {
                        "Id":1,
                        "ProductIdentifier":"BTC",
                        "Quantity":1.0,
                        "UnitPrice":5.165,
                        "TotalPrice":5.165,
                        "Links":{}
                    }
                ],
                "Links":{}
            }
        */

        let { CreditCard } = JSON.parse(data.content.toString());

        var valid = require("card-validator");
 
        var numberValidation = valid.number(CreditCard);
        
        if (!numberValidation.isPotentiallyValid) {
            console.log("Credit Card: " + CreditCard +" is invalid!");
        }
        else{
            if (numberValidation.card) {
                console.log("Credit Card: " + CreditCard +" (" + numberValidation.card.type + ") is valid");
            }else{
                console.log("Credit Card: " + CreditCard +" is valid");
            }
        }


    }, { noAck: true });
})().catch(e => console.error(e));