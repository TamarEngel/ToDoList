import axios from 'axios';
//axios.defaults.baseURL = "http://localhost:5218";
axios.defaults.baseURL =process.env.REACT_APP_API_URL;

// הוספת interceptor לשגיאות
axios.interceptors.response.use(
  response => response, 
  error => {
    console.error('API Error:', error.response ? error.response.data : error.message);
    return Promise.reject(error); 
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get(`/items`)    
    return result.data;
  },

  addTask: async(name)=>{
    const result = await axios.post(`/items`,{
      name:name,
      isComplete:false
    })
    return result.data;
  },

  setCompleted: async(id,isComplete)=>{
    const result = await axios.put(`/items/${id}?iscomplete=${isComplete}`, {});
    return result.data;
  },

  deleteTask:async(id)=>{
    const result = await axios.delete(`/items/${id}`,id)
    return result.data;
  }
};
