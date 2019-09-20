import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {MonitoringPnLayoutComponent} from './layouts';
import {AdminGuard} from '../../../common/guards';
import {MonitoringSettingsComponent, NotificationRulesPageComponent} from './components';

export const routes: Routes = [
  {
    path: '',
    component: MonitoringPnLayoutComponent,
    children: [
      {
        path: 'settings',
        canActivate: [AdminGuard],
        component: MonitoringSettingsComponent
      },
      {
        path: 'notification-rules',
        canActivate: [AdminGuard],
        component: NotificationRulesPageComponent
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
