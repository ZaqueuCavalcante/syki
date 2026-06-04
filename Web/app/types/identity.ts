export type SsoProviderType = 'AzureAd' | 'GoogleWorkspace' | 'Okta' | 'Auth0' | 'CustomOidc'

export interface GetSsoConfigurationOut {
  id: string
  providerType: SsoProviderType
  authority: string
  clientId: string
  isActive: boolean
  requireSso: boolean
  createdAt: string
}

export interface CheckSsoAvailabilityResponse {
  ssoEnabled: boolean
  ssoRequired: boolean
  providerType: SsoProviderType | null
}
