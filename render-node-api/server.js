const express = require('express');
const axios = require('axios');
require('dotenv').config();

const app = express();
//const PORT = 3000;
const PORT = process.env.PORT || 3000; // שימוש בפורט מהסביבה אם קיים


app.get('/', async (req, res) => {
  try {
    // קבלת ה-API Key מהסביבה
    const apiKey = process.env.RENDER_API_KEY;

    // שליחת בקשה ל-Render API כדי לקבל את רשימת האפליקציות
    const response = await axios.get('https://api.render.com/v1/services', {
      headers: {
        Authorization: `Bearer ${apiKey}`,
      },
    });

    // החזרת נתוני האפליקציות ב-JSON
    res.json(response.data);
  } catch (error) {
    console.error('Error fetching data from Render API:', error);
    res.status(500).json({ message: 'Error fetching data' });
  }
});

// הפעלת השרת
app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});