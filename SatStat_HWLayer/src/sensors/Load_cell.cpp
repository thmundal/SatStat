#include "Load_cell.h"

Load_cell::Load_cell(const String& name) : Sensor(name)
{
	m_load_cell = new HX711_ADC(m_dt_pin, m_sck_pin);
	m_load_cell->begin();
	m_load_cell->start(2000);
	m_load_cell->setCalFactor(103);

	sstl::Lists::add_entry<float>("gram");
	sstl::Lists::add_entry<float>("torque");
}

Load_cell::~Load_cell()
{
	delete m_load_cell;
}

void Load_cell::read_sensor()
{
	m_load_cell->update();
	m_value = m_load_cell->getData();
	sstl::Lists::set_data("gram", m_value);
	m_value = m_value / 1000 * m_length * 9.81;
	sstl::Lists::set_data("torque", m_value);
}