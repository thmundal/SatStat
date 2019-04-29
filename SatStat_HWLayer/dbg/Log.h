#ifdef _DEBUG
	struct Print_time
	{
		Print_time()
		{
			start_time = millis();
		}

		~Print_time()
		{
			end_time = millis();
			execution_time = end_time - start_time;
			String str = "Execution time: " + String(execution_time) + "ms";
			Serial.println(str);
		}

	private:
		unsigned long start_time;
		unsigned long end_time;
		unsigned long execution_time;
	};
	
	#define log(x) Serial.println(x)
	#define log_json(x) x->printTo(Serial)		
	#define scoped_timer Print_time timer
#else
	#define log(x)
	#define log_json(x)
	#define scoped_timer	
#endif