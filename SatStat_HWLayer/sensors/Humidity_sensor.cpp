#include "Humidity_sensor.h"

Humidity_sensor::Humidity_sensor(String name, int pin) : Sensor(name, pin)
{
	dht = new DHT(pin, DHT11);
	dht->begin();
}

int Humidity_sensor::read_sensor()
{		
	return dht->readHumidity();
}
