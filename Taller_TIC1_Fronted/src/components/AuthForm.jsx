import React, { useState } from 'react';
import './AuthForm.css';

const AuthForm = ({ fields, onSubmit, submitText, disabled = false, customSections = [] }) => {
  const [formData, setFormData] = useState({});
  const [errors, setErrors] = useState({});

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    // Limpiar error cuando el usuario empiece a escribir
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  const handleCustomSectionChange = (name, value) => {
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Validación básica
    const newErrors = {};
    fields.forEach(field => {
      if (field.required && !formData[field.name]) {
        newErrors[field.name] = `${field.label} es requerido`;
      }
    });

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    // Si no hay errores, enviar datos
    onSubmit(formData);
  };

  return (
    <form className="auth-form" onSubmit={handleSubmit}>
      {fields.map((field, index) => {
        // Verificar si hay una sección personalizada después de este campo
        const customSection = customSections.find(section => section.afterFieldIndex === index);
        
        return (
          <React.Fragment key={field.name}>
            <div className="form-group">
              <label htmlFor={field.name} className="form-label">
                {field.label}
                {field.required && <span className="required"> *</span>}
              </label>
              {field.type === 'select' ? (
                <select
                  id={field.name}
                  name={field.name}
                  value={formData[field.name] || ''}
                  onChange={handleInputChange}
                  className={`form-input ${errors[field.name] ? 'error' : ''}`}
                  disabled={disabled}
                >
                  <option value="">Selecciona una opción</option>
                  {field.options?.map((option) => (
                    <option key={option.value} value={option.value}>
                      {option.label}
                    </option>
                  ))}
                </select>
              ) : (
                <input
                  type={field.type}
                  id={field.name}
                  name={field.name}
                  value={formData[field.name] || ''}
                  onChange={handleInputChange}
                  className={`form-input ${errors[field.name] ? 'error' : ''}`}
                  placeholder={`Ingresa tu ${field.label.toLowerCase()}`}
                  disabled={disabled}
                />
              )}
              {errors[field.name] && (
                <span className="error-message">{errors[field.name]}</span>
              )}
            </div>
            
            {/* Renderizar sección personalizada si existe */}
            {customSection && (
              <div className="custom-section">
                {React.cloneElement(customSection.component, {
                  formData,
                  setFormData: handleCustomSectionChange,
                  errors
                })}
              </div>
            )}
          </React.Fragment>
        );
      })}
      
      <button type="submit" className="btn-submit" disabled={disabled}>
        {submitText}
      </button>
    </form>
  );
};

export default AuthForm;
