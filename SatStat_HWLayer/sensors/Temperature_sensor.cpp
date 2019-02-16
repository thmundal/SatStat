#include "Temperature_sensor.h"

Temperature_sensor::Temperature_sensor(String name, int pin) : Sensor(name, pin)
{
	dht = new DHT(pin, DHT11);
	dht->begin();
}

int Temperature_sensor::read_sensor()
{
	return dht->readTemperature();
}
