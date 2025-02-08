import 'package:flutter_test/flutter_test.dart';

import 'package:app/main.dart';

void main() {
  testWidgets('App home button', (WidgetTester tester) async {
    await tester.pumpWidget(const SykiApp());

    expect(find.text('Home'), findsOneWidget);
  });
}
