#pragma once

template <class KEY, class VALUE>
class LinkedList
{
private:
	LinkedList *m_next;
	KEY m_key;
	VALUE m_value;
	VALUE m_default;
	bool m_set;
	int count(int current);
public:
	LinkedList();
	LinkedList(const LinkedList<KEY, VALUE> &other);
	~LinkedList();

	LinkedList<KEY, VALUE> &operator=(const LinkedList<KEY, VALUE> &other);
	VALUE &operator[](const int);

	void append(const KEY &key, const VALUE &value);
	VALUE &get(const KEY &key);
	void setDefault(const VALUE &value);
	int count();
};

// Jon tester
template<class KEY, class VALUE>
inline int LinkedList<KEY, VALUE>::count(int current)
{
	if (m_next != 0)
	{
		return m_next->count(++current);
	}

	return ++current;
}

template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::LinkedList()
{
	m_next = 0;
	m_set = false;
}

template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::LinkedList(const LinkedList<KEY, VALUE>& other)
{
	m_next = 0;
	*this = other;
}

template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::~LinkedList()
{
	delete m_next;
}

template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>& LinkedList<KEY, VALUE>::operator=(const LinkedList<KEY, VALUE>& other)
{
	delete m_next;
	m_key = other.m_key;
	m_value = other.m_value;
	m_set = other.m_set;
	m_next = new LinkedList<KEY,VALUE>;
	*m_next = *other.m_next;
}

// Jon tester
template<class KEY, class VALUE>
inline VALUE& LinkedList<KEY, VALUE>::operator[](int index)
{	
	if (index == 0)
	{
		return m_value;
	}
	if (m_next != 0)
	{		
		return m_next->operator[](--index);
	}

	return m_default;	
}

template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::append(const KEY &key, const VALUE &value)
{
	if (!m_set)
	{
		m_key = key;
		m_value = value;
		m_set = true;
	}
	else if (m_next == 0)
	{
		m_next = new LinkedList;
		m_next->append(key, value);
		m_next->setDefault(m_default);
	}
	else
	{
		m_next->append(key, value);
	}
}

template<class KEY, class VALUE>
inline VALUE &LinkedList<KEY, VALUE>::get(const KEY &key)
{
	if (m_key == key)
		return m_value;
	if (m_next != 0)
		return m_next->get(key);
	return m_default;
}

template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::setDefault(const VALUE & value)
{
	m_default = value;
	if (m_next != 0)
		m_next->setDefault(value);
}

// Jon tester
template<class KEY, class VALUE>
inline int LinkedList<KEY, VALUE>::count()
{	
	if (m_next != 0)
	{
		return m_next->count(1);
	}
	else if (m_set)
	{
		return 1;
	}
	
	return 0;
}
