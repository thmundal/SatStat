#include "Temp_sensor.h"

int Temp_sensor::read_sensor()
{
	DHT.read11(Sensor::pin);
	return DHT.temperature;	
}
