// https://docs.cypress.io/api/table-of-contents

describe('My First Test', () => {
  it('Visits the app root url', () => {
    cy.visit('/')
    cy.contains('h1', 'Welcome to Your Vue.js App')
  })
})

describe('Login', () => {
  beforeEach(() => {
    cy.visit('/login') // Replace '/login' with the actual login page URL
  })

  it('should display login form', () => {
    cy.get('input[name="email"]').should('be.visible')
    cy.get('input[name="password"]').should('be.visible')
    cy.get('button[type="submit"]').should('be.visible')
  })

  it('should login with valid email and password', () => {
    cy.get('input[name="email"]').type('example@example.com') // Replace with valid email
    cy.get('input[name="password"]').type('password123') // Replace with valid password
    cy.get('button[type="submit"]').click()

    // Add assertions for successful login, such as redirect or displayed user information
    // For example:
    cy.url().should('include', '/dashboard') // Replace '/dashboard' with the expected URL after successful login
    cy.contains('Welcome, John Doe') // Replace 'John Doe' with the expected user name
  })

  it('should display error message for invalid login credentials', () => {
    cy.get('input[name="email"]').type('invalid@example.com') // Replace with invalid email
    cy.get('input[name="password"]').type('wrongpassword') // Replace with invalid password
    cy.get('button[type="submit"]').click()

    // Add assertions for error message display
    // For example:
    cy.contains('Invalid email or password') // Replace with the expected error message
  })
})