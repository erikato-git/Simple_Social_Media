import { useState } from 'react';

type InputField = {
  id: string;
  label: string;
};

const inputFields: InputField[] = [
  { id: 'field1', label: 'Field 1' },
  { id: 'field2', label: 'Field 2' },
  { id: 'field3', label: 'Field 3' },
];

export default function App() {
  const [fieldValues, setFieldValues] = useState<{ [key: string]: string }>({});

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const inputId = event.currentTarget.id;
    const inputValue = event.currentTarget.value;

    setFieldValues((prevValues) => ({
      ...prevValues,
      [inputId]: inputValue,
    }));
  };

  return (
    <div>
      {inputFields.map((field) => (
        <div key={field.id}>
          <label htmlFor={field.id}>{field.label}</label>
          <input type="text" id={field.id} onChange={handleInputChange} />
        </div>
      ))}

      <button onClick={() => console.log(fieldValues)}>Submit</button>
    </div>
  );
}