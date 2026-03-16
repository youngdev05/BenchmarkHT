import pandas as pd
import matplotlib.pyplot as plt
import glob
import os
import re

# Настройка шрифта для корректного отображения кириллицы (опционально)
plt.rcParams['font.sans-serif'] = ['Segoe UI', 'Arial', 'DejaVu Sans']
plt.rcParams['axes.unicode_minus'] = False

current_dir = os.path.dirname(os.path.abspath(__file__))
os.chdir(current_dir)

print(f"Текущая папка: {current_dir}")

csv_files = glob.glob("bin/Release/net8.0/BenchmarkDotNet.Artifacts/results/*-report.csv")
if not csv_files:
    print("CSV файлы не найдены!")
    exit()

latest_csv = max(csv_files, key=os.path.getctime)
print(f"Читаем файл: {latest_csv}")

df = pd.read_csv(latest_csv, delimiter=';')


def clean_numeric(value):
    if pd.isna(value) or value == 'NA':
        return float('nan')
    cleaned = re.sub(r'[^\d.\-]', '', str(value))
    try:
        return float(cleaned) if cleaned else float('nan')
    except:
        return float('nan')


df['Mean'] = df['Mean'].apply(clean_numeric)
df['Allocated'] = df['Allocated'].apply(clean_numeric)

df = df[df['Mean'].notna()]
df = df[df['Allocated'].notna()]

if df.empty:
    print("Нет данных для отображения!")
    exit()

df['Method'] = df['Method'].str.replace("'", "")
df = df.sort_values('Mean', ascending=True)

print("\n" + "=" * 70)
print("📊 СВОДНАЯ ТАБЛИЦА РЕЗУЛЬТАТОВ")
print("=" * 70)
print(f"{'Метод':<20} {'Время (мкс)':<15} {'Память (B)':<15}")
print("-" * 70)
for _, row in df.iterrows():
    print(f"{row['Method']:<20} {row['Mean']:<15.2f} {row['Allocated']:<15.0f}")
print("=" * 70)

try:
    plt.figure(figsize=(14, 8))
    colors = ['#FF6B6B' if 'EF' in m else '#4ECDC4' if 'Dapper' in m else '#45B7D1' for m in df['Method']]
    plt.barh(df['Method'], df['Mean'], color=colors)
    plt.title('Сравнение производительности ORM (CRUD операции)\nSQLite, .NET 8', fontsize=14, fontweight='bold')
    plt.xlabel('Время (мкс)', fontsize=12)
    plt.ylabel('Метод', fontsize=12)
    plt.grid(axis='x', linestyle='--', alpha=0.7)

    for i, v in enumerate(df['Mean']):
        plt.text(v + 50, i, f'{v:.0f}', va='center', fontsize=9)

    plt.tight_layout()
    plt.savefig('orm_comparison.png', dpi=300, bbox_inches='tight')
    print("\nграфик времени сохранен: orm_comparison.png")
    plt.close()  # Закрываем фигуру, чтобы освободить память
except Exception as e:
    print(f"\nршибка при сохранении графика времени: {e}")

try:
    plt.figure(figsize=(14, 8))
    colors = ['#FF6B6B' if 'EF' in m else '#4ECDC4' if 'Dapper' in m else '#45B7D1' for m in df['Method']]
    plt.barh(df['Method'], df['Allocated'], color=colors)

    plt.title('выделение памяти (байты на операцию)', fontsize=14, fontweight='bold')
    plt.xlabel('Байты', fontsize=12)
    plt.ylabel('Метод', fontsize=12)
    plt.grid(axis='x', linestyle='--', alpha=0.7)

    for i, v in enumerate(df['Allocated']):
        plt.text(v + 1000, i, f'{v:.0f} B', va='center', fontsize=9)

    plt.tight_layout()
    plt.savefig('orm_memory.png', dpi=300, bbox_inches='tight')
    print("график памяти сохранен: orm_memory.png")
    plt.close()
except Exception as e:
    print(f"\nошибка при сохранении графика памяти: {e}")

print("\nвизуализация все")