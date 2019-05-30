#pragma once
#include "Sensor.h"
#include "../containers/SADM_functions.h"

class Pos_sensor : public Sensor
{
public:
	Pos_sensor(const String& name);

	void read_sensor() override;

private:
	const int m_pin = A0;
	const float m_lv = 1023.0f * 10.0f / 100.0f;		// Lowest value
	const float m_hv = 1023.0f - m_lv;					// Highest value
	const float m_range = m_hv - m_lv;                  // Value range
	const float m_val_inc_per_deg = m_range / 360.0f;	// Value increase per degree rotation
	int m_val;											// Raw value from sensor
	int m_last_val;										// Last raw value from sensor	
	float m_pos;										// Position in degrees
};