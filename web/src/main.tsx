import './index.css'
import ReactDOM from "react-dom/client"
import LoginPage from "./pages/LoginPage.tsx"
import NotFoundPage from "./pages/NotFoundPage.tsx"
import AcademicCampiPage from "./pages/AcademicCampiPage.tsx"
import AuthProvider from "./components/custom/AuthProvider.tsx"
import Layout from "./components/custom/Layout.tsx"

import { BrowserRouter as Router, Routes, Route } from 'react-router';
import App from './App.tsx'
import ProtectedRoute from './components/custom/ProtectedRoute.tsx'
import React from 'react'
import AcademicCoursesPage from './pages/AcademicCoursesPage.tsx'

const root = document.getElementById("root")

ReactDOM.createRoot(root!).render(
  <React.StrictMode>
      <Router>
          <AuthProvider>
              <Routes>
                  <Route path="/" element={<App />} />
                  <Route path="/login" element={<LoginPage />} />

                  <Route element={<ProtectedRoute roles={['Academic']}><Layout /></ProtectedRoute>}>
                      <Route path="/academic/campi" element={<AcademicCampiPage />} />
                      <Route path="/academic/courses" element={<AcademicCoursesPage />} />
                  </Route>

                  <Route path="*" element={<NotFoundPage />} />
              </Routes>
          </AuthProvider>
      </Router>
  </React.StrictMode>
)
