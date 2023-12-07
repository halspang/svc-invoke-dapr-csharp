param name string
param location string = resourceGroup().location
param tags object = {}
param containerAppName string = ''
 
resource policies 'Microsoft.App/containerApps/resiliencyPolicies@2023-08-01-preview' = {
  name: name
  location: location
  tags: tags
  parent: containerApp
  properties: {
    timeoutPolicy: {
        responseTimeoutInSeconds: 15
        connectionTimeoutInSeconds: 5
    }
    httpRetryPolicy: {
        maxRetries: 5
        retryBackOff: {
          initialDelayInMilliseconds: 1000
          maxIntervalInMilliseconds: 10000
        }
        matches: {
            errors: [
                '5xx'
            ]
        }
    } 
    circuitBreakerPolicy: {
        consecutiveErrors: 5
        intervalInSeconds: 10
        maxEjectionPercent: 50
    }
  }
}
 
resource containerApp 'Microsoft.App/containerApps@2022-03-01' existing = {
  name: containerAppName
}
