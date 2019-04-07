#pragma once
#include "Sensor.h"
#include "../../libraries/dht.h"

/**
*	The Temp_hum_sensor is an example of a class for a specific sensor, in this case the DHT11 temperature and humidity sensor.
*	This class inherits the members of the sensor class, and includes the DHT library.
*	It has it's own constructor, and overrides the read_sensor method as required.
*	This is to support the specific sensor, as well as keeping the same structure as every other sensor to make sure creation and reading of different kinds of sensors
*	work the exact same way.
*/
class Temp_hum_sensor : public Sensor
{
public:
	Temp_hum_sensor(const String& name, const int& pin);	
	const Sensor::sub_list_type& read_sensor();	
private:	
	dht DHT;
};