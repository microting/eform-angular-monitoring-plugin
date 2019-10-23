import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {MonitoringPnLayoutComponent} from './layouts';
import {AdminGuard, AuthGuard, PermissionGuard} from '../../../common/guards';
import {MonitoringSettingsComponent, NotificationRulesPageComponent} from './components';
import {MonitoringPnClaims} from './const/monitoring-pn-claims.const';

export const routes: Routes = [
  {
    path: '',
    component: MonitoringPnLayoutComponent,
    canActivate: [PermissionGuard],
    data: {requiredPermission: MonitoringPnClaims.accessMonitoringPlugin},
    children: [
      {
        path: 'settings',
        component: MonitoringSettingsComponent,
        canActivate: [AdminGuard]
      },
      {
        path: 'notification-rules',
        component: NotificationRulesPageComponent,
        canActivate: [AuthGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MonitoringPnRoutingModule {
}
