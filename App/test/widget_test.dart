import 'package:flutter_test/flutter_test.dart';

import 'package:app/main.dart';

void main() {
  testWidgets('Acompanhe seu progresso', (WidgetTester tester) async {
    await tester.pumpWidget(const SykiApp());

    expect(find.text('Acompanhe seu progresso'), findsOneWidget);
  });
}
