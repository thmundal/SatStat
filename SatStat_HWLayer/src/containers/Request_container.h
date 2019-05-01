#pragma once
#include "Func_ptr.h"
#include "../../lib/LinkedList/LinkedList.h"
#include "Request_functions.h"

class Request_container
{
public:	
	// Constructor
	Request_container();

	// Checks
	bool exists(const String& key);
	bool has_params(const String& key);

	// Getters
	Func_ptr<void>& get_no_param(const String& key);
	Func_ptr<void, Json_container<JsonObject>&>& get_with_param(const String& key);

private:
	void append(const String& key, Func_ptr<void> func);
	void append(const String& key, Func_ptr<void, Json_container<JsonObject>&> func);

	LinkedList<String, Func_ptr<void>> no_param;
	LinkedList <String, Func_ptr<void, Json_container<JsonObject>&>> with_param;
};

