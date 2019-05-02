#include "Instruction_container.h"

/**
*	Constructor appending every available instruction to the list.
*/
Instruction_container::Instruction_container()
{	
	SADM_functions::init_stepper();

	m_available_instructions.append("set_gear_ratio",
		new Instruction
		(
			"set_gear_ratio",
			parse_params
			(
				Uf_param("ratio", "float")
			),
			SADM_functions::set_ratio
		)
	);

	m_available_instructions.append("auto_rotate",
		new Instruction
		(
			"auto_rotate",
			parse_params
			(				
				Uf_param("enable", "bool")
			),
			SADM_functions::set_auto_rotate
		)
	);

	m_available_instructions.append("rotate_steps",
		new Instruction
		(
			"rotate_steps",
			parse_params
			(				
				Uf_param("steps", "int")
			),
			SADM_functions::rotate
		)
	);

	m_available_instructions.append("rotate_degrees", 
		new Instruction
		(
			"rotate_degrees",
			parse_params
			(				
				Uf_param("degrees", "float")
			),
			SADM_functions::rotate
		)
	);
}

/**
*	Returns a spesific instruction in the list.
*/
Instruction* Instruction_container::get(const String & ins)
{
	return m_available_instructions.get(ins);
}

/**
*	Returns the entire list of available instructions.
*/
LinkedList<String, Instruction*>& Instruction_container::get_available_instructions()
{	
	return m_available_instructions;
}