import { Policies } from '~/policies'
import type { PolicyName } from '~/policies'
import { createSharedComposable } from '@vueuse/core'

const _usePolicy = () => {
  const { account } = useUserAccount()

  function can(policyName: PolicyName): ComputedRef<boolean> {
    return computed(() => {
      if (!account.value) return false

      const policy = Policies[policyName]

      return policy.requirements.every((req) => {
        switch (req.type) {
          case 'hasUserType':
            return account.value!.userType === req.value
          case 'hasAnyUserType':
            return req.values.includes(account.value!.userType)
          case 'hasPermission':
            return account.value!.permissions.includes(req.permissionId)
          case 'hasAnyPermission':
            return req.permissionIds.some(id => account.value!.permissions.includes(id))
        }
      })
    })
  }

  return { can }
}

export const usePolicy = createSharedComposable(_usePolicy)
