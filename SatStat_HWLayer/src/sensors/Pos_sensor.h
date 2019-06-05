#pragma once
#include "Sensor.h"
#include "../containers/SADM_functions.h"

/**
*	Position sensor class.
*	Calibrates, interprets and provide angular rotation in degrees.
*/
class Pos_sensor : public Sensor
{
public:
	Pos_sensor(const String& name);						// Constructor
	static void tare();									// Set pos to 0
	void read_sensor() override;						// Reads the sensor

private:
	static const int m_pin;								// Sensor analog input pin
	const float m_lv = 1023.0f * 10.0f / 100.0f;		// Lowest value
	const float m_hv = 1023.0f - m_lv;					// Highest value
	const float m_range = m_hv - m_lv;                  // Value range
	const float m_val_inc_per_deg = m_range / 360.0f;	// Value increase per degree rotation
	static int m_val;									// Raw value from sensor
	static int m_last_val;								// Last raw value from sensor	
	static float m_pos;									// Position in degrees
};