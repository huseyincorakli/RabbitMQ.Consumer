using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://ymsclgok:bW2XZJGz5gmJSCs7D0NFtcX_RxbHvkR-@sparrow.rmq.cloudamqp.com/ymsclgok");


using IConnection connection = factory.CreateConnection(); //Bağlantı Aktifleştirme
using IModel channel = connection.CreateModel(); //Kanal açma

string exchangeName = "fanout-exchange-example";

channel.ExchangeDeclare(exchange: exchangeName, ExchangeType.Fanout);

Console.Write("Queue Name:");
string queueName=Console.ReadLine();

channel.QueueDeclare(
    queue: queueName,
    exclusive: false);

channel.QueueBind(
    queue: queueName,
    exchange: exchangeName,
    routingKey: string.Empty);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();