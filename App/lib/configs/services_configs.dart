import 'package:get_it/get_it.dart';
import 'package:app/auth/auth_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

final getIt = GetIt.instance;

void addServicesConfigs() {
  getIt.registerSingleton(const FlutterSecureStorage());
  getIt.registerSingleton(AuthService());

  // getIt.registerLazySingleton(() => CreatePendingUserRegisterClient());
}
