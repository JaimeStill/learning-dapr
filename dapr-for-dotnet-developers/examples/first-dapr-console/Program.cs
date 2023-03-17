using Dapr.Client;

const string storeName = "statestore";
const string key = "counter";

DaprClient daprClient = new DaprClientBuilder().Build();
int counter = await daprClient.GetStateAsync<int>(storeName, key);

while (true)
{
    Console.WriteLine($"Counter = {counter++}");

    await daprClient.SaveStateAsync(storeName, key, counter);
    await Task.Delay(1000);
}