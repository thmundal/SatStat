#include "Instruction_container.h"

Instruction_container::Instruction_container()
{	
	SADM_functions::init_stepper();

	m_available_instructions.append("auto_rotate",
		new Instruction
		(
			parse_signature
			(
				"auto_rotate",
				Uf_param("enable", "bool")
			),
			SADM_functions::set_auto_rotate
		)
	);

	m_available_instructions.append("rotate_steps",
		new Instruction
		(
			parse_signature
			(
				"rotate_steps",
				Uf_param("steps", "int")
			),
			SADM_functions::rotate
		)
	);

	m_available_instructions.append("rotate_deg", 
		new Instruction
		(
			parse_signature
			(
				"rotate_deg",
				Uf_param("deg", "float")
			),
			SADM_functions::rotate
		)
	);	

	m_available_instructions.append("subscribe",
		new Instruction
		(
			parse_signature
			(
				"subscribe",
				Uf_param("data_list", "JsonArray")
			),
			Subscriber_functions::subscribe
		)
	);

	m_available_instructions.append("unsubscribe",
		new Instruction
		(
			parse_signature
			(
				"unsubscribe",
				Uf_param("data_list", "JsonArray")
			),
			Subscriber_functions::unsubscribe
		)
	);
}

Instruction* Instruction_container::get(const String & ins)
{
	return m_available_instructions.get(ins);
}

LinkedList<String, Instruction*>& Instruction_container::get_available_instructions()
{	
	return m_available_instructions;
}