#pragma once
#include "Sensor.h"
#include "../../lib/DHT/DHT_U.h"

/**
*	The Temp_hum_sensor is an example of a class for a specific sensor, in this case the DHT22 temperature and humidity sensor.
*	This class inherits from the Sensor class, and includes the DHT library.
*	It has it's own constructor, and overrides the read_sensor method as required.
*	This is to support the specific sensor, as well as keeping the same structure as every other sensor to make sure creation and reading of different kinds of sensors
*	work the exact same way.
*/
class Temp_hum_sensor : public Sensor
{
public:
	Temp_hum_sensor(const String& name);	
	~Temp_hum_sensor();

	void read_sensor() override;	
private:	
	DHT_Unified* dht;

	const int m_pin = 5;
};